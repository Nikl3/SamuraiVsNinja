using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : SingeltonPersistant<SceneMaster>
{
    private AsyncOperation asyncOperation;

    private float flickeringSpeed = 1f;
    private float fakeLoadTime = 1f;
    private bool isFading;
    private bool isHowToPlayImageFading;
    private bool isAnimatingText;
    private Image screenFadeImage;
    private Image howToPlayImage;
    private Text loadText;
    private Text pressAnyKeyText;


    protected override void Awake()
    {
        base.Awake();

        screenFadeImage = transform.Find("FadeImage").GetComponent<Image>();
        howToPlayImage = transform.Find("HowToPlayImage").GetComponent<Image>();
        loadText = transform.Find("LoadText").GetComponent<Text>();
        pressAnyKeyText = howToPlayImage.transform.Find("PressAnyKeyText").GetComponent<Text>();
        screenFadeImage.fillAmount = 1f;
        howToPlayImage.fillAmount = 0f;
    }

    private void Start()
    {
        howToPlayImage.gameObject.SetActive(false);
        loadText.enabled = false;
        pressAnyKeyText.enabled = false;

        FadeScreenImage(0);
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

    private int RandomizeNumbers(int minValue, int maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue);
    }

    public void ExitGame(Action action)
    {
        StartCoroutine(IExitingGame(action));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(ILoadSceneAsync(sceneIndex));
    }

    public void AnimateText(Text textToAnimate, float flickeringSpeed)
    {
        StartCoroutine(IAnimateText(textToAnimate, flickeringSpeed));
    }

    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        RandomizeFillMethod();
        StartCoroutine(IFadeScreenImage(targetFillAmount, 1f));
    }

    private void AnimateHowToPlayImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        StartCoroutine(IAnimateHowToPlayImage(targetFillAmount, fadeSpeed));
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

        yield return new WaitUntil(() => !isFading);

        howToPlayImage.gameObject.SetActive(true);
        pressAnyKeyText.enabled = true;
        loadText.enabled = true;

        AnimateHowToPlayImage(1, 0.5f);

        AnimateText(pressAnyKeyText, flickeringSpeed);

        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => !isHowToPlayImageFading);

        //yield return new WaitForSeconds(fakeLoadTime);

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                loadText.enabled = false;

                if (Input.anyKeyDown)
                {
                  
                    isAnimatingText = false;
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        pressAnyKeyText.enabled = false;
        AnimateHowToPlayImage(0, 0.5f);
        yield return new WaitUntil(() => !isHowToPlayImageFading);

        howToPlayImage.gameObject.SetActive(false);

        loadText.enabled = true;

        yield return new WaitForSeconds(fakeLoadTime);

        loadText.enabled = false;
 
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

        while (isAnimatingText)
        {
            textToAnimate.enabled = false;
            yield return new WaitForSeconds(flickeringSpeed);
            textToAnimate.enabled = true;
            yield return new WaitForSeconds(flickeringSpeed);
        }
    }

    private IEnumerator IAnimateHowToPlayImage(float targetFillAmount, float fadeSpeed)
    {
        isHowToPlayImageFading = true;

        while (howToPlayImage.fillAmount != targetFillAmount)
        {
            howToPlayImage.fillAmount += howToPlayImage.fillAmount < targetFillAmount ? (1f / fadeSpeed) * Time.unscaledDeltaTime : -(1f / fadeSpeed) * Time.unscaledDeltaTime;
            yield return null;
        }

        isHowToPlayImageFading = false;
        yield return null;
    }
}
