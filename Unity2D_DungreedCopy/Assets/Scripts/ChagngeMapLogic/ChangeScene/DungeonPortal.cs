using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPortal : MonoBehaviour
{
    public bool                     eatPlayer = false;
    public string                   tranferMapName;   // 이동할 맵의 이름

    private PoolManager             poolManager;
    private PlayerController        player;
    private DungeonPortalController dungeonPortalController;

    private void Awake()
    {
        player                  = FindObjectOfType<PlayerController>();
        dungeonPortalController = FindObjectOfType<DungeonPortalController>();
    }
    public void Setup(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void ThePortalEatPlayer()
    {
        eatPlayer = true;

        PlayerController.instance.onUI = true;
        PlayerController.instance.spriteRenderer.color = new Color(1, 1, 1, 0);
        PlayerController.instance.weaponRenderer.color = new Color(1, 1, 1, 0);

        UIManager.instance.fadeOn = true;
    }
    public void FalseToEatPlayer()
    {
        eatPlayer = false;

        player.curSceneName = tranferMapName;

        FadeEffectController.instance.OnFade(FadeState.FadeOut);

        StartCoroutine(ChangeScene());
    }
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(FadeEffectController.instance.fadeTime);
        poolManager.DeactivePoolItem(gameObject);
        SceneManager.LoadScene(tranferMapName);
    }
}
