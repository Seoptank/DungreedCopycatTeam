using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stats
{
    [HideInInspector]
    public float HP;
    [HideInInspector]
    public float STEMINA;
    [HideInInspector]
    public int   DashCount;
}
public abstract class Entity : MonoBehaviour
{
    private Stats       stats;      // ĳ���� ����
    public  Entity      target;     // ���� ���

    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;
    }
    public float STEMINA
    {
        set => stats.STEMINA = Mathf.Clamp(value, 0, MaxSTEMINA);
        get => stats.STEMINA;
    }

    public int DashCount
    {
        set => stats.DashCount = Mathf.Clamp(value, 0, MaxDashCount);
        get => stats.DashCount;
    }

    public abstract float MaxHP { get; }               // �ִ� ü��
    public abstract float MaxSTEMINA { get; }          // �ִ� ���׹̳�
    public abstract float RecoverySTEMINA { get; }     // ���׹̳� �ʴ� ȸ����
    public abstract float consumptionSTEMINA { get; }  // ���׹̳� �ʴ� ȸ����
    public abstract int   MaxDashCount{ get; }         // �ִ� ��� ī��Ʈ

    protected void Setup()
    {
        HP = MaxHP;
        STEMINA = MaxSTEMINA;
        DashCount = MaxDashCount;

        StartCoroutine("Recovery");
    }

    // YS: �ʴ� ü�� ȸ��
    protected IEnumerator Recovery()
    {
        while(true)
        {
            if (STEMINA < MaxSTEMINA) STEMINA += RecoverySTEMINA;

            yield return new WaitForSeconds(3);
        }
    }

    public abstract void ConsumptionSteminaAndCount(float consumptionSTEMINA, int consumptionDashCount);

    public abstract void TakeDamage(float damage);

}
