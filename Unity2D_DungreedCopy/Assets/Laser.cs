using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private GameObject parentObj;
    
    public void DeactivateLaser()
    {
        parentObj.SetActive(false);
    }
}
