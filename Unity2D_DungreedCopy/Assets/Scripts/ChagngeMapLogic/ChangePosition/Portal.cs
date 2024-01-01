using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private PortalStartPoint    portalStartPoint;

    private PlayerController        player;
    private FadeEffectController    fade;
    
    [Header("�ش� MarkCurMap ����")]
    [SerializeField]
    private MarkCurMap              markCurMap;
    public string                   dungeonMapMoveDir;      // ������: R, ����: L, �Ʒ�:D, ��:U

    [Header("���� �̵��� dungeon������Ʈ ����")]
    [SerializeField]
    private GameObject              nextDungeon;
    private string                  transferDungeonName;    // YS: �̵��� ���� �̸�


    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            transferDungeonName     = nextDungeon.name;
            player.curDungeonName   = transferDungeonName;
            fade.OnFade(FadeState.FadeOut);
            StartCoroutine(portalStartPoint.ChangePlayerPosition());
            markCurMap.dungeonMapDir = dungeonMapMoveDir;
        }
    }
}
