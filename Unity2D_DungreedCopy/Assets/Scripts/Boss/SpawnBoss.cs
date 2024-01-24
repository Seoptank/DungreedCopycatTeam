using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField]
    private Transform           bossCamViewPos;
    [SerializeField]
    private float               camViewSmoothSpeed;
    [SerializeField]
    private SpriteRenderer[]    spritesBoss;
    [SerializeField]
    private float               limitTime;

    [Header("보스 소개 UI 컨트롤")]
    [SerializeField]
    private TextMeshProUGUI TextBossNameUI;
    [SerializeField]
    private TextMeshProUGUI TextBossNicknameUI;
    [SerializeField]
    private string          stringBossName;
    [SerializeField]
    private string          stringBossNickname;
    [SerializeField]
    private Image           BossIntroduceImageTop;
    [SerializeField]
    private Image           BossIntroduceImageBottom;
    [SerializeField]
    private AnimationCurve  activeCurve;

    private CircleCollider2D circleCollider2D;
    private UIEffectManager uiEffectManager;
    private MainCameraController mainCam;
    private PlayerController player;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();

        uiEffectManager = FindObjectOfType<UIEffectManager>();
        mainCam = FindObjectOfType<MainCameraController>();
        player = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        for (int i = 0; i < spritesBoss.Length; ++i)
        {
            uiEffectManager.StarteSetting(spritesBoss[i]);
        }

        TextBossNameUI.text     = stringBossName;
        TextBossNicknameUI.text = stringBossNickname;
    }

    private void Update()
    {
        if (player.playerMeetsBoss == true)
        {
            for (int i = 0; i < spritesBoss.Length; ++i)
            {
                StartCoroutine(uiEffectManager.UIFade(spritesBoss[i], 0, 1));
            }

            IntroduceBoss();
        }
    }

    private void IntroduceBoss()
    {
        // mainCam 이동 bossCamViewPos로 이동
        mainCam.ChangeView(bossCamViewPos, camViewSmoothSpeed);

        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageTop, 0, 1));
        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageBottom, 0, 1));

        StartCoroutine(uiEffectManager.UIFade(TextBossNameUI, 0, 1));
        StartCoroutine(uiEffectManager.UIFade(TextBossNicknameUI, 0, 1));

        StartCoroutine(ExitIntroduceBoss());
        
    }

    private IEnumerator ExitIntroduceBoss()
    {
        yield return new WaitForSeconds(1f);

        limitTime = 0.2f;

        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageTop, 1, 0));
        StartCoroutine(uiEffectManager.UIFade(BossIntroduceImageBottom, 1, 0));

        StartCoroutine(uiEffectManager.UIFade(TextBossNameUI, 1, 0));
        StartCoroutine(uiEffectManager.UIFade(TextBossNicknameUI, 1, 0));

        StartCoroutine(ReturnCamPos());
    }
    private IEnumerator ReturnCamPos()
    {
        yield return new WaitForSeconds(2f);
        player.playerMeetsBoss  = false; 

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.playerMeetsBoss = true;
            circleCollider2D.enabled = false;
        }
    }
}


