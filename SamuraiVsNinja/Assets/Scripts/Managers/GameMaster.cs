﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    #region VARIABLES

    public Transform UIManager { get; private set; }
    public Transform MainMenuUI { get; private set; }

    private AsyncOperation asyncOperation;

    private readonly float flickeringSpeed = 1f;
    private readonly float fakeLoadTime = 1f;
    private bool isFading;
    private bool isAnimatingText;
    private Image screenFadeImage;
    private Image howToPlayImage;
    private Text messageText;

    #endregion VARIABLES

    protected override void Awake()
    {
        base.Awake();

        UIManager = transform.Find("UIManager");
        MainMenuUI = UIManager.Find("MainMenuUI");

        screenFadeImage = UIManager.transform.Find("FadeImage").GetComponent<Image>();
        howToPlayImage = UIManager.transform.Find("HowToPlayImage").GetComponent<Image>();
        messageText = howToPlayImage.transform.GetComponentInChildren<Text>();
        screenFadeImage.fillAmount = 1f;
        howToPlayImage.fillAmount = 0f;

        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            MainMenuUI.gameObject.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "DevScene - Niko")
        {
            MainMenuUI.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        howToPlayImage.gameObject.SetActive(false);
        messageText.enabled = false;

        FadeScreenImage(0);
    }

    private int RandomizeNumbers(int minValue, int maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue);
    }

    private void RandomizeFillMethod()
    {
        var randomIndex = RandomizeNumbers(0, 4);
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

        randomIndex = 0;
    }

    public void ExitGame(Action action)
    {
        StartCoroutine(IExitingGame(action));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(ILoadSceneAsync(sceneIndex));
    }

    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        RandomizeFillMethod();
        StartCoroutine(IFadeScreenImage(targetFillAmount, 1f));
    }

    public void AnimateText(Text textToAnimate, float flickeringSpeed)
    {
        StartCoroutine(IAnimateText(textToAnimate, flickeringSpeed));
    }

    #region COROUTINES

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

        yield return new WaitUntil(() => !isFading);

        messageText.enabled = true;
        messageText.text = "LOADING...";
        howToPlayImage.gameObject.SetActive(true);

        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(fakeLoadTime);

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                if (!isAnimatingText)
                {
                    AnimateText(messageText, flickeringSpeed);
                    messageText.text = "PRESS ANY KEY";
                }

                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        isAnimatingText = false;
        howToPlayImage.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(fakeLoadTime);

        FadeScreenImage(0);
    }

    private IEnumerator IExitingGame(Action action)
    {
        FadeScreenImage(1);

        yield return new WaitUntil(() => !isFading);

        action.Invoke();
    }

    private IEnumerator IAnimateText(Text textToAnimate, float flickeringSpeed)
    {
        isAnimatingText = true;
        messageText.enabled = true;

        while (isAnimatingText)
        {
            textToAnimate.enabled = false;
            yield return new WaitForSecondsRealtime(flickeringSpeed);
            textToAnimate.enabled = true;
            yield return new WaitForSecondsRealtime(flickeringSpeed);
        }
    }

    #endregion COROUTINES
}
