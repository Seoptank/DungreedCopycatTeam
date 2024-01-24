using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject  laserPrefab;

    public Animator     anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartAttackAni()
    {
        anim.SetBool("IsAttack", true);    
    }

    public void ActivateLaser()
    {
        laserPrefab.SetActive(true);
    }
    public void DeactivateLaser()
    {
        laserPrefab.SetActive(false);
    }

    public void StopAttackAni()
    {
        anim.SetBool("IsAttack", false);    
    }
}
