using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }
public class FadeEffectController : MonoBehaviour
{
    static public FadeEffectController instance;

    public float           fadeTime;
    [SerializeField]
    private AnimationCurve  fadeCurve;
    private Image           imageFade;
    private FadeState       fadeState;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            imageFade = GetComponent<Image>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnFade(FadeState state )
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                StartCoroutine(DeactivateFadeImage());
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private IEnumerator DeactivateFadeImage()
    {
        yield return new WaitForSeconds(fadeTime);
        this.gameObject.SetActive(false);
    }

    public IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(0, 1));
            yield return StartCoroutine(Fade(1, 0));

            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }
    public IEnumerator Fade(float start,float end)
    {
        float curTime = 0;
        float percent = 0;

        while(percent < 1)
        {
            curTime += Time.deltaTime;
            percent = curTime / fadeTime;

            Color color     = imageFade.color;
            color.a         = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            imageFade.color = color;

            yield return null;
        }
    }
}
