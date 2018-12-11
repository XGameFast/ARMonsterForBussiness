using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBasic : MonoBehaviour {
    public BaseController baseController;
    public CanvasGroup canvasGroup;
    private float currentFadeProgress;
    private bool isFadeIn =false;
    public virtual void StartView()
    {

    }

    public virtual void EndView()
    {

    }

    public void FadeIn()
    {
        if(!isFadeIn)
        {
            isFadeIn = true;
            StartCoroutine(ExcuteFadeIn());
        }
    }

    private IEnumerator ExcuteFadeIn()
    {
        while(isFadeIn && currentFadeProgress < 1)
        {
            currentFadeProgress += Time.deltaTime;
            canvasGroup.alpha = currentFadeProgress;
            yield return null;
        }

    }

    public void FadeOut()
    {
        if (isFadeIn)
        {
            isFadeIn = false;
            StartCoroutine(ExcuteFadeOut());
        }
    }

    private IEnumerator ExcuteFadeOut()
    {
        while (!isFadeIn && currentFadeProgress > 0)
        {
            currentFadeProgress -= Time.deltaTime;
            canvasGroup.alpha = currentFadeProgress;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
