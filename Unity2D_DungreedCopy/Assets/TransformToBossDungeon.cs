using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransformToBossDungeon : MonoBehaviour
{

    [SerializeField]
    private GameObject              spriteKey_F;
    [SerializeField]
    private string                  transferBossStageName;
    [SerializeField]
    private bool                    isActiveTransferKey = false;

    private KeyCode                 transferBossKey = KeyCode.F;

    private PlayerController        player;
    private FadeEffectController    fade;
    private MapController           map;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        fade   = FindObjectOfType<FadeEffectController>();
        map    = FindObjectOfType<MapController>();
    }
    private void Update()
    {
        spriteKey_F.SetActive(isActiveTransferKey);

        if (isActiveTransferKey)
        {
            if(Input.GetKeyDown(transferBossKey))
            {
                // ������ �� �̸����� ����
                player.curSceneName = transferBossStageName;

                // dungeonName List����
                map.dungeonNames.Clear();

                // ���̵�ƿ� ȿ��
                fade.OnFade(FadeState.FadeOut);

                StartCoroutine(TranferBossStage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            isActiveTransferKey = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isActiveTransferKey = false;
            //spriteKey_F.SetActive(isActiveTransferKey);
        }
    }
    private IEnumerator TranferBossStage()
    {
        yield return new WaitForSeconds(fade.fadeTime);
        SceneManager.LoadScene(transferBossStageName);
    }
}
