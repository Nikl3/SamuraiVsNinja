using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CURRENT_GAME_STATE
{
    MAIN_MENU,
    LOCAL_GAME,
    ONLINE_GAME
}

public class GameMaster : SingeltonPersistant<GameMaster>
{
    #region VARIABLES

    public CURRENT_GAME_STATE CurrentGameState { get; private set; }

    private AsyncOperation asyncOperation;
    private readonly float flickeringSpeed = 1f;
    private float fakeLoadTime = 1f;
    private bool isFading;
    private bool isAnimatingText;
    private Image screenFadeImage;
    private Image howToPlayImage;
    private Text messageText;

    private Coroutine fadeScreenImage;
    private Coroutine loadSceneAsync;
    private Coroutine exitingGame;
    private Coroutine animateText;

    public AudioMixer AudioMixer;

    #endregion VARIABLES

    protected override void Awake()
    {
        base.Awake();

        screenFadeImage = UIManager.Instance.transform.Find("FadeImage").GetComponent<Image>();
        howToPlayImage = UIManager.Instance.transform.Find("HowToPlayImage").GetComponent<Image>();
        messageText = howToPlayImage.transform.GetComponentInChildren<Text>();
        screenFadeImage.fillAmount = 1f;
        howToPlayImage.fillAmount = 0f;

       
    }
    private void Start()
    {
        CheckCurrentScene();

        howToPlayImage.gameObject.SetActive(false);
        messageText.enabled = false;

        FadeScreenImage(0);

        AudioMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;

        SetVolumeChannels();
    }

    private int RandomizeNumbers(int minValue, int maxValue)
    {
        return UnityEngine.Random.Range(minValue, maxValue);
    }
    private void CheckCurrentScene()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "MainMenuScene":
                ChangeGameState(CURRENT_GAME_STATE.MAIN_MENU);
                break;

            case "DevScene - Niko":
                ChangeGameState(CURRENT_GAME_STATE.LOCAL_GAME);
                break;

            case "DevScene - Mathias":
                ChangeGameState(CURRENT_GAME_STATE.ONLINE_GAME);
                break;
        }
    }
    /// <summary>
    /// Invoke methods: "ClearTargets", "ClearPlayerInfoContainer", "ClearJoinedPlayers"
    /// </summary>
    private void Clear()
    {
        CameraEngine.Instance.ClearTargets();

        foreach (var playerData in PlayerDataManager.Instance.PlayerDatas)
        {
            if (playerData.Player != null)
            {
                //playerData.SpawnedPlayer.DespawnPlayer();
            }
        }
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

    public void ChangeGameState(CURRENT_GAME_STATE newGameState)
    {
        CurrentGameState = newGameState;

        switch (CurrentGameState)
        {
            case CURRENT_GAME_STATE.MAIN_MENU:
                UIManager.Instance.SetMainMenuUI();
                break;

            case CURRENT_GAME_STATE.LOCAL_GAME:
                UIManager.Instance.SetLevelUI();
                break;

            case CURRENT_GAME_STATE.ONLINE_GAME:
                UIManager.Instance.SetOnlineUI();
                break;

            default:

                break;
        }
    }
    public void ExitGame(Action action)
    {
        if(exitingGame == null)
           exitingGame = StartCoroutine(IExitingGame(action));
    }
    public void LoadScene(int sceneIndex)
    {
        if (loadSceneAsync == null)
        {
            Clear();
            loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex));
        }        
    }
    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f)
    {
        RandomizeFillMethod();
        if (fadeScreenImage == null)
            fadeScreenImage = StartCoroutine(IFadeScreenImage(targetFillAmount, 0.5f));
    }
    public void AnimateText(Text textToAnimate, float flickeringSpeed)
    {
        if (animateText == null)
        animateText = StartCoroutine(IAnimateText(textToAnimate, flickeringSpeed));
    }

    public void SetVolumeChannels()
    {
        AudioMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
        AudioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        AudioMixer.SetFloat("Sfx", PlayerPrefs.GetFloat("Sfx"));
    }

    public void SaveChannelValues()
    {
        float masterVolume = 0f;
        float musicVolume = 0f;
        float sfxVolume = 0f;

        AudioMixer.GetFloat("Master", out masterVolume);
        AudioMixer.GetFloat("Music", out musicVolume);
        AudioMixer.GetFloat("Sfx", out sfxVolume);

        PlayerPrefs.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("Sfx", sfxVolume);
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

        fadeScreenImage = null;
    }
    private IEnumerator ILoadSceneAsync(int sceneIndex)
    {
        FadeScreenImage(1);

        yield return new WaitUntil(() => !isFading);

        messageText.enabled = true;
        messageText.text = "LOADING...";
        howToPlayImage.gameObject.SetActive(sceneIndex != 0 ? true : false);

        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;


        yield return new WaitForSecondsRealtime(fakeLoadTime = howToPlayImage.gameObject.activeSelf ? fakeLoadTime : 0f);

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                if (!isAnimatingText)
                {
                    AnimateText(messageText, flickeringSpeed);
                    messageText.text = "PRESS ANY KEY";
                }

                if (Input.anyKeyDown || !howToPlayImage.gameObject.activeSelf)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        isAnimatingText = false;
        howToPlayImage.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(fakeLoadTime);

        CheckCurrentScene();

        FadeScreenImage(0);

        loadSceneAsync = null;
    }
    private IEnumerator IExitingGame(Action action)
    {
        FadeScreenImage(1);

        yield return new WaitUntil(() => !isFading);

        action.Invoke();

        exitingGame = null;
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

        animateText = null;
    }

    #endregion COROUTINES
}
