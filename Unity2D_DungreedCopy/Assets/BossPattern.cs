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

    [Header("BossPattern")]
    [SerializeField]
    private int             patternCount;
    [SerializeField]
    private int             maxPatternCount;
    [SerializeField]        
    private int             minPatternCount;

    [Header("HeadAttack")]
    [SerializeField]
    private GameObject      headBulletPrefab;
    [SerializeField]
    private int             angleInterval = -10;    // ��� = �ݽð� ����, ���� = �ð� ����
    [SerializeField]
    private int             fireDirCount = 4;       // bullet�� ������ ������ ����
    [SerializeField]
    private float           fireRateTime = 0.2f;    // bullet�� ���� �ð� ����
    [HideInInspector]
    public  PoolManager     headAttackPoolManager;
    [SerializeField]
    private float           headAttackMinTime = 3.0f;
    [SerializeField]
    private float           headAttackMaxTime = 5.0f;
    [SerializeField]
    private float           headAttackTime = 0;
    [SerializeField]
    private bool            isHeadAttack;


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
    public List<GameObject>     swordList = new List<GameObject>();

    [Header("HandsAttack")]
    private GameObject          selectedHand;
    [SerializeField]
    private GameObject          leftHand;
    [SerializeField]
    private GameObject          rightHand;
    [SerializeField]
    private float               waitHandAttackTime;
    [SerializeField]
    private float               handsMoveTime;
    [SerializeField]
    private int                 count;
    [SerializeField]
    private int                 maxCount;
    [SerializeField]
    private int                 minCount;

    private GameObject player;

    private void Awake()
    {
        headAttackPoolManager       = new PoolManager(headBulletPrefab);
        bossSwordSpawnPoolManager   = new PoolManager(bossSwordSpawnPrefab);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(isHeadAttack)
        {
            headAttackTime += Time.deltaTime;

            if(headAttackTime > Random.Range(headAttackMinTime,headAttackMaxTime))
            {
                isHeadAttack = false;

                if(!isHeadAttack)
                {
                    StartCoroutine(HeadAttackTimeReturnZero());
                }
            }
        }
    }

    private IEnumerator PatternRoutain()
    {
        // ���� ���ǿ� ���������� ��������
        while (true)
        {
            yield return new WaitForSeconds(15f);

            int randumIndex = Random.Range(0, 3);

            if(randumIndex == 0)
            {
                ChangeBossState(BossState.HandsAttack);
                
                // ���� ��� �ð��� ���� �ڿ� Idel�����ϵ���
            }
            else if(randumIndex == 1)
            {
                ChangeBossState(BossState.HeadAttack);
            }
            else
            {
                ChangeBossState(BossState.HeadAttack);
            }
        }
    }

    private IEnumerator HandsAttack()
    {
        count = Random.Range(minCount, maxCount);
        while(count >= 0)
        {
            yield return new WaitForSeconds(waitHandAttackTime);
            count--;

            int randomIndex = Random.Range(0, 2);

            if(randomIndex == 0)
            {
                selectedHand = leftHand;
            }
            else
            {
                selectedHand = rightHand;
            }

            Vector2 startPos = selectedHand.transform.position;
            Vector2 targetPos = new Vector2(selectedHand.transform.position.x, player.transform.position.y);

            float elapsedTime = 0f;

            while(elapsedTime < handsMoveTime)
            {
                selectedHand.transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / handsMoveTime);
                elapsedTime += Time.deltaTime;
             
                if(elapsedTime >= handsMoveTime)
                {
                    selectedHand.GetComponent<BossHands>().StartAttackAni();
                }
                yield return null;
            }
        }
    }
    private IEnumerator HeadAttackTimeReturnZero()
    {
        yield return new WaitForSeconds(3f);
        headAttackTime = 0;

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

    private IEnumerator HeadAttack()
    {
        int fireAngle = 0;  // �ʱⰪ�� 0��
        isHeadAttack = true;

        while (isHeadAttack == true)
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
        // ������ ����ϴ� ���� ���� 
        StopCoroutine(bossState.ToString());

        // ���� ����
        bossState = newState;
        
        // ���ο� ���� ���
        StartCoroutine(bossState.ToString());
    }
}
