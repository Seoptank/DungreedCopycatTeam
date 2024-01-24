using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState 
{
    Idle = 0,
    HeadAttack,    
    HandsAttack,   
    SwordAttack    
}
public class BossPattern : MonoBehaviour
{
    public BossState   bossState;

    [Header("HeadAttack")]
    [SerializeField]
    private GameObject      headBulletPrefab;
    [SerializeField]
    private int             angleInterval = -10;    // 양수 = 반시계 방향, 음수 = 시계 방향
    [SerializeField]
    private int             fireDirCount = 4;       // bullet이 나가는 방향의 갯수
    [SerializeField]
    private float           fireRateTime = 0.2f;    // bullet의 생성 시간 제어
    [SerializeField]
    private float           fireRoutainTime;        // bullet의 생성 기간 제어
    [SerializeField]
    private bool            isHeadAttack = false;  
    [HideInInspector]
    public  PoolManager     headAttackPoolManager;

    [Header("SwordAttack")]
    [SerializeField]
    private GameObject          bossSwordSpawnPrefab;
    [HideInInspector]
    public  PoolManager         bossSwordSpawnPoolManager;
    [SerializeField]
    private float               bossSwordSpawnDelayTime;
    [SerializeField]
    private Transform[]         spawnTransforms;
    public bool                 isSpawnAllSword = false;

    [Header("HandsAttack")]
    [HideInInspector]
    public GameObject           selectedHand;
    [SerializeField]
    private GameObject          rightHand;
    [SerializeField]
    private GameObject          lefttHand;
    [SerializeField]
    private int                 handsAttackRoutainCount;
    [SerializeField]
    private float               waitForOneRoutaionTime;     // 한루틴이 끝나는 시간동안 기다릴 시간
    [SerializeField]
    private float               moveTime;                   // 손이 이동하는 시간
    public bool                 isHandAttack;

    private GameObject          player;
    
    private void Awake()
    {
        headAttackPoolManager       = new PoolManager(headBulletPrefab);
        bossSwordSpawnPoolManager   = new PoolManager(bossSwordSpawnPrefab);
        
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        //ChangeBossState(BossState.HeadAttack);
        ChangeBossState(BossState.HandsAttack);
    }

    private void Update()
    {
        if (isHeadAttack)
        {
            fireRoutainTime -= Time.deltaTime;

            if(fireRoutainTime <= 0)
            {
                isHeadAttack = false;

                if(!isHeadAttack)
                {
                    fireRoutainTime = Random.Range(3, 6);
                }
            }
        }
    }

    private IEnumerator SwordAttack()
    {
        for (int i = 0; i < spawnTransforms.Length; ++i)
        {
            yield return new WaitForSeconds(bossSwordSpawnDelayTime);
            GameObject bossSwordSpawn = bossSwordSpawnPoolManager.ActivePoolItem();
            bossSwordSpawn.transform.position = spawnTransforms[i].position;
            bossSwordSpawn.transform.rotation = transform.rotation;
            bossSwordSpawn.GetComponent<BossSwordSpawnEffect>().Setup(bossSwordSpawnPoolManager);
        }
    }

    private IEnumerator HandsAttack()
    {
        handsAttackRoutainCount = Random.Range(3, 6);

        while(handsAttackRoutainCount >= 0)
        {

            yield return new WaitForSeconds(waitForOneRoutaionTime);
            handsAttackRoutainCount--;

            int randomIndex = Random.Range(0, 2);

            if(randomIndex == 0)
            {
                selectedHand = lefttHand;

            }
            else
            {
                selectedHand = rightHand;
            }

            Vector2 startPosition = selectedHand.transform.position;
            Vector2 targetPosition = new Vector2(selectedHand.transform.position.x, player.transform.position.y);

            float elapsedTime = 0f;

            while (elapsedTime < moveTime)
            {
                selectedHand.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;

                if(elapsedTime >= moveTime)
                {
                    selectedHand.GetComponent<BossHandAttack>().StartAttackAni();
                }
                yield return null;
            }
        }
    }

    private IEnumerator HeadAttack()
    {
        int fireAngle = 0;  // 초기값은 0도

        isHeadAttack = true;
        fireRoutainTime = fireRoutainTime = Random.Range(3, 6);

        while(isHeadAttack)
        {

            for (int i = 0; i < fireDirCount; ++i)
            {
                fireAngle += i + 90;

                GameObject tempObj = headAttackPoolManager.ActivePoolItem();

                Vector2 dir = new Vector2(Mathf.Cos(fireAngle * Mathf.Deg2Rad), Mathf.Sin(fireAngle * Mathf.Deg2Rad));

                tempObj.transform.right = dir;
                tempObj.transform.position = transform.position;
                tempObj.GetComponent<BossHeadBullet>().Setup(headAttackPoolManager);
            }


            yield return new WaitForSeconds(fireRateTime);

            fireAngle += angleInterval;

            if (fireAngle > 360) fireAngle -= 360;
        }
    }

    private void ChangeBossState(BossState newState)
    {
        // 이전에 재생하던 상태 종료 
        StopCoroutine(bossState.ToString());

        // 상태 변경
        bossState = newState;
        
        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }
}
