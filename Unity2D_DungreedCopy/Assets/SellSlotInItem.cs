using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellSlotInItem : MonoBehaviour
{
    [SerializeField]
    private ItemSO item;
    [SerializeField]
    private InventorySO inventory;

    private void Update()
    {
        // ���� �ڵ������� ���߿� 
        inventory.AddItem(item, 1);
    }
}
