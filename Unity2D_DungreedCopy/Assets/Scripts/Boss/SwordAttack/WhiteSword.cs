using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSword : MonoBehaviour
{
    [SerializeField]
    private GameObject  stuckSwordPrefab;
    [SerializeField]
    private GameObject  swordHitEffectPrefab;
    [SerializeField]
    private float       moveSpeed;

    private PoolManager poolManager;
    private PoolManager stuckSwordPoolManager;
    private PoolManager swordHitEffectPoolManager;
    private void Awake()
    {
        stuckSwordPoolManager       = new PoolManager(stuckSwordPrefab);
        swordHitEffectPoolManager   = new PoolManager(swordHitEffectPrefab);
    }

    private void OnApplicationQuit()
    {
        stuckSwordPoolManager.DestroyObjcts();
    }

    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Vector2.up * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            poolManager.DeactivePoolItem(gameObject);
            SpawnHitEffect();
            SpawnStuckSword();
        }
    }

    private void SpawnHitEffect()
    {
        GameObject hitEffect = swordHitEffectPoolManager.ActivePoolItem();
        hitEffect.transform.position = transform.position;
        hitEffect.transform.rotation = hitEffect.transform.rotation;
        hitEffect.GetComponent<EffectPool>().Setup(swordHitEffectPoolManager);
    }
    private void SpawnStuckSword()
    {
        GameObject stuckSword = stuckSwordPoolManager.ActivePoolItem();
        stuckSword.transform.position = transform.position;
        stuckSword.transform.rotation = transform.rotation;
        stuckSword.GetComponent<StuckSword>().Setup(stuckSwordPoolManager);
    }
}
