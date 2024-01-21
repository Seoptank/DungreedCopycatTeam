using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadBullet : MonoBehaviour
{
    [Header("HeadBullet ����")]
    [SerializeField]
    private int         dam;
    [SerializeField]
    private float       moveSpeed;


    [Header("BossBulletGameObject")]
    private PoolManager     bossBulletEffectPoolManager;
    [SerializeField]
    private GameObject      bossBulletEffectPrefab;

    private Rigidbody2D             rigidbody2D;
    private PlayerStats             playerStats;
    private PoolManager             poolManager;
    private MainCameraController    mainCam;
    private BoxCollider2D           thisBound;

    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    private void Awake()
    {
        
        bossBulletEffectPoolManager = new PoolManager(bossBulletEffectPrefab);

        rigidbody2D     = GetComponent<Rigidbody2D>(); 
        playerStats     = FindObjectOfType<PlayerStats>();
        mainCam         = FindObjectOfType<MainCameraController>();
        
        thisBound = mainCam.bound;
    }

    private void OnApplicationQuit()
    {
        bossBulletEffectPoolManager.DestroyObjcts();
    }
    private void Update()
    {
        rigidbody2D.velocity = moveSpeed * transform.right;

        if(!isInsideBound(this.transform.position))
        {
            DeactivateBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            // bullet ��Ȱ��ȭ
            DeactivateBullet();

            // Effect Ȱ��ȭ
            ActivateBossBulletEffect();
            
            // Player���� ������ �ִ� ����
            playerStats.DecreaseHP(dam);
        }
    }

    private bool isInsideBound(Vector2 bulletPos)
    {
        if (thisBound != null)
            return thisBound.bounds.Contains(bulletPos);
        return false;
    }

    private void DeactivateBullet()
    {
        poolManager.DeactivePoolItem(gameObject);
    }
    private void ActivateBossBulletEffect()
    {
        GameObject bossBulletEffect = bossBulletEffectPoolManager.ActivePoolItem();
        bossBulletEffect.transform.position = this.transform.position;
        bossBulletEffect.transform.rotation = transform.rotation;
        bossBulletEffect.GetComponent<EffectPool>().Setup(bossBulletEffectPoolManager);
    }
}
