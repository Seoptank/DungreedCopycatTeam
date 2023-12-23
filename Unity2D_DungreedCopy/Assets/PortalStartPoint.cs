using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalStartPoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D   targetBound;            // YS: �̵��� ���� ī�޶� �ٿ��
    public string           startPointDungeonName;  // YS: ��ŸƮ ������ �� �̸�

    private PlayerController        player;
    private FadeEffectController    fade;
    private MainCameraController    mainCam;

    private void Awake()
    {
        player  = FindObjectOfType<PlayerController>();
        fade    = FindObjectOfType<FadeEffectController>();
        mainCam = FindObjectOfType<MainCameraController>();

    }
    public IEnumerator ChangePlayerPosition()
    {
        yield return new WaitForSeconds(fade.fadeTime);
            
        if (startPointDungeonName == player.curDungeonName)
        {
            // �÷��̾� ��ġ �̵�
            player.transform.position = this.transform.position;

            // ī�޶� ��ġ �̵�
            mainCam.transform.position = new Vector3(this.transform.position.x,
                                                     this.transform.position.y,
                                                     mainCam.transform.position.z);

            // ���̵� ȿ��
            fade.OnFade(FadeState.FadeIn);

            // �ٿ�� �缳��
            mainCam.SetBound(targetBound);
        }
    }
}
