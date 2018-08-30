﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : Singelton<SceneMaster>
{
    private AsyncOperation asyncOperation;

    private float delayTime = 1f;
    private bool isFading;
    private Image screenFadeImage;

    protected void Awake()
    {
        screenFadeImage = transform.Find("FadeImage").GetComponent<Image>();
        screenFadeImage.fillAmount = 1f;
    }

    private void Start()
    {
        Invoke("DelayStart", delayTime);
    }

    private void DelayStart()
    {
        RandomizeFillMethod();
        FadeScreenImage(0);
    }

    private void RandomizeFillMethod()
    {
        int randomIndex = RandomizeNumbers(0, 4);
        switch (randomIndex)
        {
            case 0:
                screenFadeImage.fillMethod = Image.FillMethod.Horizontal;
                screenFadeImage.fillOrigin = RandomizeNumbers(0, 1);
                break;
            case 1:
                screenFadeImage.fillMethod = Image.FillMethod.Vertical;
                screenFadeImage.fillOrigin = RandomizeNumbers(0, 1);
                break;
            case 2:
                screenFadeImage.fillMethod = Image.FillMethod.Radial90;
                screenFadeImage.fillClockwise = RandomizeNumbers(0, 1) == 1 ? true : false;
                screenFadeImage.fillOrigin = RandomizeNumbers(0, 3);
                break;
            case 3:
                screenFadeImage.fillMethod = Image.FillMethod.Radial180;
                screenFadeImage.fillClockwise = RandomizeNumbers(0, 1) == 1 ? true : false;
                screenFadeImage.fillOrigin = RandomizeNumbers(0, 3);
                break;
            case 4:
                screenFadeImage.fillMethod = Image.FillMethod.Radial360;
                screenFadeImage.fillClockwise = RandomizeNumbers(0, 1) == 1 ? true : false;
                screenFadeImage.fillOrigin = RandomizeNumbers(0, 3);
                break;
            default:
                
                break;
        }
    }

    private int RandomizeNumbers(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(ILoadSceneAsync(sceneIndex));
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        StartCoroutine(IFadeScreenImage(targetFillAmount, 1f));
    }

    private IEnumerator IFadeScreenImage(float targetFillAmount, float fadeSpeed)
    {
        isFading = true;
        screenFadeImage.raycastTarget = true;

        while (screenFadeImage.fillAmount != targetFillAmount)
        {
            screenFadeImage.fillAmount += screenFadeImage.fillAmount < targetFillAmount ? (1f / fadeSpeed) * Time.unscaledDeltaTime : -(1f / fadeSpeed) * Time.unscaledDeltaTime;
            yield return null;
        }

        screenFadeImage.raycastTarget = false;
        isFading = false;
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex)
    {
        FadeScreenImage(1);

        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if(asyncOperation.progress == 0.9f)
            {
                yield return new WaitUntil(() => !isFading);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        FadeScreenImage(0);
    }
}
