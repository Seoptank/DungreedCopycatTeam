using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("StrartPoint ����")]
    [SerializeField]
    private PortalStartPoint        portalStartPoint;

    private PlayerController        player;
    private FadeEffectController    fade;
    
    [Header("�ش� MarkCurMap ����")]
    [SerializeField]
    private MarkCurMap              markCurMap;
    public string                   dungeonMapMoveDir;      // ������: R, ����: L, �Ʒ�:D, ��:U

    [Header("���� �̵��� dungeon������Ʈ ����")]
    [SerializeField]
    private GameObject              nextDungeon;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            UIManager.instance.fadeOn = true;

            markCurMap.dungeonMapDir = dungeonMapMoveDir;
            nextDungeon.SetActive(true);
            PlayerController.instance.onUI = true;
            player.curDungeonName   = nextDungeon.GetComponent<DungeonName>().dungeonName;
            player.curDungeonNum = nextDungeon.GetComponent<DungeonName>().dungeonNum;
            FadeEffectController.instance.OnFade(FadeState.FadeOut);
            StartCoroutine(portalStartPoint.ChangePlayerPosition());
        }
    }
}
