using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public ItemSO ������;

    [SerializeField]
    private InventorySO �κ��丮;

    void Update()
    {
        �κ��丮.AddItem(������, 1);
    }
}
