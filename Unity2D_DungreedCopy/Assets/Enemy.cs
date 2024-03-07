using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy ����")]
    private float curHP;
    [SerializeField]
    private float maxHP;


    [SerializeField]
    private float   timeToReturnOriginColor = 0.3f;

    private Color originColor;
    private Color color;

    private SpriteRenderer  spriteRenderer;
    private HPBar           healthBar;

    private void Start()
    {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        healthBar       = GetComponentInChildren<HPBar>();

        curHP = maxHP;
        healthBar.UpdateHPBar(curHP, maxHP);

        originColor = spriteRenderer.color;
        color = Color.red;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            // �ǰݽ� �÷� ����
            //spriteRenderer.color = color;
            spriteRenderer.color = collision.gameObject.GetComponent<WeponInfo>().textColor;

            // �ǰ� ���� ���󺹱� �ڷ�ƾ �Լ� ����
            StartCoroutine(ReturnColor());

            // �� ü�� ����
            TakeDamage(collision.gameObject.GetComponent<WeponInfo>().curATK,   
                       collision.gameObject.GetComponent<WeponInfo>().textColor);
            
            // Enemy ü�¹� �ֽ�ȭ
            healthBar.UpdateHPBar(curHP, maxHP);
        }
    }

    private IEnumerator ReturnColor()
    {
        yield return new WaitForSeconds(timeToReturnOriginColor);
        spriteRenderer.color = originColor;
    }

    private void TakeDamage(int dam,Color color)
    {
        Color textColor = Color.white;

        if(curHP >0)
        {
            curHP -= dam;

            textColor = color;
            Debug.Log(textColor);
        }
    }

}
