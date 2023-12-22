using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YS
public class DungeonPortalController : MonoBehaviour
{
    [SerializeField]
    private GameObject dungeonPortalPrefab;

    private PoolManager dungeonPortalPoolMnager;

    private void Awake()
    {
        dungeonPortalPoolMnager = new PoolManager(dungeonPortalPrefab);
    }
    private void OnApplicationQuit()
    {
        dungeonPortalPoolMnager.DestroyObjcts();
    }

    private void ActiveDungeonPortal()
    {
        GameObject dungeonPortal = dungeonPortalPoolMnager.ActivePoolItem();
        dungeonPortal.transform.position = new Vector3(transform.position.x,-1.5f,transform.position.z);
        dungeonPortal.transform.rotation = transform.rotation;
        dungeonPortal.GetComponent<DungeonPortal>().Setup(dungeonPortalPoolMnager);
    }

    // �÷��̾�� �浹�� MemoryPool�� DungeonPortal �ҷ�����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            ActiveDungeonPortal();
        }
    }
}
