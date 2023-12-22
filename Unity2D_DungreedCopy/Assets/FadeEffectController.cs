using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }
public class FadeEffectController : MonoBehaviour
{
    public  float           fadeTime;
    [SerializeField]
    private AnimationCurve  fadeCurve;
    private Image           imageFade;
    private FadeState       fadeState;

    private void Awake()
    {
        imageFade = GetComponent<Image>();
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
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