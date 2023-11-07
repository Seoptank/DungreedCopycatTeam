using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    //zzzz
    [Header("무기 종류")]
    private KeyCode weapon1 = KeyCode.Alpha1;
    public GameObject[] Weapons;
    public bool[] hasWeapons;
    public Weapon weapon;
    public Sprite[] sprites;

    public GameObject MeleePos1;
    public SpriteRenderer MeleeSpr1;
    public GameObject MeleePos2;
    public SpriteRenderer MeleeSpr2;
    public bool SwitchPos = true;

    void Awake()
    {
        //weapon.GetComponent<Weapon>();
        MeleeSpr1.GetComponent<SpriteRenderer>();
        MeleeSpr2.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (SwitchPos == true)
            {
                //MeleeSpr1.sprite = sprites[weapon.value];
                MeleePos1.SetActive(true);
                MeleePos2.SetActive(false);
                SwitchPos = false;
            }
            else if (SwitchPos == false)
            {
                MeleePos1.SetActive(false);
                MeleePos2.SetActive(true);
                SwitchPos = true;
            }
        }
    }
}
