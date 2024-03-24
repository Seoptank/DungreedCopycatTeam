using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack: MonoBehaviour
{
    [Header("화살 생성을 위한 변수들")]
    [SerializeField]
    private GameObject  arrowPrefab;        // 생성할 화살 프리팹
    private float       arrowSpeed = 50f;  // 화살의 속도
    private Transform   arrowSpawn;

    private PoolManager arrowpoolManager;        

    private void Awake()
    {
        arrowpoolManager = new PoolManager(arrowPrefab);
        arrowSpawn = transform.GetChild(0).GetComponent<Transform>();
    }

    private void OnApplicationQuit()
    {
        arrowpoolManager.DestroyObjcts();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)&& PlayerController.instance.canAttack && !PlayerController.instance.onUI)
        {
            Fire();
            StartCoroutine(PlayerController.instance.AbleToAttack());
        }
    }
    void Fire()
    {
        GameObject arrow = arrowpoolManager.ActivePoolItem();
        arrow.transform.position = arrowSpawn.position;
        arrow.transform.rotation = transform.rotation;
        Rigidbody2D rigidbody = arrow.GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * arrowSpeed;
        arrow.GetComponent<Arrow>().Setup(arrowpoolManager);
    }
}
