using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : SingeltonPersistant<SceneMaster>
{
    private AsyncOperation asyncOperation;

    private float fakeLoadTime = 1f;
    private bool isFading;
    private Image screenFadeImage;
    private Text loadText;
    public GameObject ControllerIM;

    protected override void Awake()
    {
        base.Awake();

        screenFadeImage = transform.Find("FadeImage").GetComponent<Image>();
        loadText = transform.Find("LoadText").GetComponent<Text>();
        screenFadeImage.fillAmount = 1f;
        loadText.enabled = false;
    }

    private void Start()
    {
        FadeScreenImage(0);
        ControllerIM.SetActive(false);

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

    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        RandomizeFillMethod();
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

        yield return new WaitUntil(() => !isFading);
        //
        ControllerIM.SetActive(true);
        loadText.enabled = true;
        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(fakeLoadTime);

        ControllerIM.SetActive(false);
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadText.enabled = false;
        FadeScreenImage(0);
    }

    private IEnumerator IExitingGame(Action action)
    {
        FadeScreenImage(1);

        yield return new WaitUntil(() => !isFading);

        action.Invoke();
    }
}
