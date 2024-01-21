using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState 
{
    Idle = 0,
    HeadAttack,    
    ArmsAttack,   
    SwordAttack    
}
public class BossPattern : MonoBehaviour
{
    public BossState   bossState;

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

    private void Awake()
    {
        headAttackPoolManager       = new PoolManager(headBulletPrefab);
        bossSwordSpawnPoolManager   = new PoolManager(bossSwordSpawnPrefab);

    }
    private void Start()
    {
        ChangeBossState(BossState.SwordAttack);
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

        while(true)
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
