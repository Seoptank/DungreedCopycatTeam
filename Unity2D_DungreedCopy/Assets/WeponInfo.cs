using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponInfo : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField]
    private int     minATK,maxATK;
    
    [HideInInspector]
    public int      curATK;
    [HideInInspector]
    public Color    textColor;

    private PlayerStats stats;
    private System.Random random = new System.Random();

    private void Awake()
    {
        stats = FindObjectOfType<PlayerStats>();

        CalculateDamage();
    }
    private void CalculateDamage()
    {
        // ���� ���ݷ� ���
        int randomATK = Random.Range(minATK, maxATK + 1);

        //ũ��Ƽ�� �ߵ���
        if (IsCritical())
        {
            // ũ��Ƽ�ý� ���ݷ� = �ִ� �������� + (�ִ빫������ * 0.5) + �÷��̾� ���ݷ�
            curATK = maxATK + (int)(maxATK * 0.5f) + stats.ATK;
            Debug.Log("ũ��Ƽ�� �ߵ�!. ���ݷ�: " + curATK);
            textColor = Color.yellow;
        }
        else
        {
            // �Ϲݰ��ݽ� ���ݷ� = �ִ� �������� + �÷��̾� ���ݷ�
            curATK = randomATK + stats.ATK;
            Debug.Log("���ݷ�: " + curATK);
            textColor = Color.red;
        }
    }

    public bool IsCritical()
    {
        return (random.NextDouble() < stats.CRI);
    }
}
