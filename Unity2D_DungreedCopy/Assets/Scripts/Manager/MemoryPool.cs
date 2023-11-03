using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: YS

public class MemoryPool : MonoBehaviour
{
    //�޸� Ǯ�� �����Ǵ� ������Ʈ ����
    public class PoolItem
    {
        public bool isActive;               // "gameObject"�� Ȱ�� ��Ȱ��ȭ ����
        public GameObject gameObject;       // ȭ�鿡 ���̴� ���� ������Ʈ
    }

    private int increaseCount = 5;          // ������Ʈ ������ Instantiate()�� �߰��� �����Ǵ� ������Ʈ ����
    private int maxCount;                   // ���� ����Ʈ�� ��ϵ� ������Ʈ ����
    private int activeCount;                // ���� ���ӿ� ���ǰ��ִ�(Ȱ��ȭ) ������Ʈ ����

    private GameObject      poolObject;     // ������ƮǮ������ �����ϴ� ���ӿ�����Ʈ ������
    private List<PoolItem>  poolItemList;   // �����ϴ� ��� ������Ʈ�� �����ϴ� ������

    // ����: Ŭ���� �̸��� ���� �̸��� �Լ��� "������"�� �ش�Ŭ������ ������ �����ϰ�
    //       �޸𸮰� �Ҵ�� �� �ڵ����� ȣ��
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    // ����: increaseCount������ ������Ʈ ���� 
    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);

            poolItemList.Add(poolItem);
        }
    }

    //����: ���� ��������(Ȱ��/��Ȱ��) ��� ������Ʈ ����
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    // ����: poolItemList�� ����Ǿ��ִ� ��� ������Ʈ�� Ȱ��ȭ�ؼ� ���
    //       ����, ��� ������Ʈ�� Ȱ��ȭ���̸� InstantiateObjects()�� �߰� ����
    public GameObject ActivePoolItem()
    {
        if (poolItemList == null) return null;

        // ���� �����ؼ� �����ϴ� ��� ������Ʈ ������ ���� Ȱ��ȭ�� ������ ������Ʈ ���� ��
        // ��� Ȱ��ȭ �����̸� ���ο� ������Ʈ �ʿ�
        if(maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    // ����: ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ����
    public void DeactivatePoolItem(GameObject removeItem)
    {
        if (poolItemList == null || removeItem == null) return;
        
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeItem)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    // ����: ���ӿ� ������� ��� ������Ʈ�� ��Ȱ��ȭ ���·� ��ȯ
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;
        
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;

    }
}