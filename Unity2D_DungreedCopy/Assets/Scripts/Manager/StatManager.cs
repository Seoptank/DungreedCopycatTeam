using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    [HideInInspector]
    public float    HP;     // �÷��̾� ü��
    [HideInInspector]       
    public int      DC;     // �÷��̾� ��� ī��Ʈ
}

public abstract class StatManager : MonoBehaviour
{
    private Stats       stats;                // ĳ���� ����

    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;
    }
    public int DC
    {
        set => stats.DC = Mathf.Clamp(value, 0, MaxDC);
        get => stats.DC;
    }

    public abstract float       MaxHP { get; }              // �ִ� ü��
    public abstract int         MaxDC { get; }              // �ִ� ��� ī��Ʈ
    
    public void Setup()
    {
        HP = MaxHP;
        DC = MaxDC;
    }
}
