using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalStartPoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D   targetBound;            // YS: �̵��� ���� ī�޶� �ٿ��
    [SerializeField]
    private  string         startingMapName;
    [SerializeField]
    private DungeonName     nextDungeon;

    private PlayerController        player;
    private FadeEffectController    fade;
    private MainCameraController    mainCam;
    private MapController           map;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
        mainCam = FindObjectOfType<MainCameraController>();
        map     = FindObjectOfType<MapController>();
    }
    private void Start()
    {
        startingMapName = nextDungeon.dungeonName;
    }

    public IEnumerator ChangePlayerPosition()
    {
        yield return new WaitForSeconds(fade.fadeTime);
            
        if (startingMapName == player.curDungeonName)
        {
            // �÷��̾� ��ġ �̵�
            player.transform.position = this.transform.position;

            // ī�޶� ��ġ �̵�
            mainCam.transform.position = new Vector3(this.transform.position.x,
                                                     this.transform.position.y,
                                                     mainCam.transform.position.z);

            if (!map.dungeonNames.Contains(startingMapName))
            {
                map.dungeonNames.Add(startingMapName);
                Debug.Log(startingMapName + "�� ����Ʈ�� �߰��ƽ��ϴ�.");
            }

            // ���̵� ȿ��
            fade.OnFade(FadeState.FadeIn);

            // �ٿ�� �缳��
            mainCam.SetBound(targetBound);
        }
    }
}
