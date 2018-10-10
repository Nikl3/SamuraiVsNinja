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
    private readonly float fakeLoadTime = 1f;
    private bool isFading;
    private bool isAnimatingText;
    private Image screenFadeImage;
    private GameObject loadImageObject;
    private Text messageText;

    private Coroutine fadeScreenImage;
    private Coroutine loadSceneAsync;
    private Coroutine exitingGame;
    private Coroutine animateTextCoroutine;

    public AudioMixer AudioMixer;

    #endregion VARIABLES

    public bool IsLoadingScene { get; private set; }
   
    protected override void Awake()
    {
        base.Awake();

        screenFadeImage = UIManager.Instance.transform.Find("FadeImage").GetComponent<Image>();
        loadImageObject = UIManager.Instance.transform.Find("LoadImage").gameObject;
        messageText = loadImageObject.transform.GetComponentInChildren<Text>();
        screenFadeImage.fillAmount = 1f;   
    }
    private void Start()
    {
        CheckCurrentScene();

        loadImageObject.SetActive(false);
        messageText.enabled = false;

        FadeScreenImage(0);

        AudioMixer.updateMode = AudioMixerUpdateMode.Normal;
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
   
    private void Clear()
    {
        CameraEngine.Instance.ClearTargets();
     
        foreach (var playerData in PlayerDataManager.Instance.PlayerDatas)
        {
            if (playerData.HasJoined)
            {
                Destroy(playerData.PlayerInfo.gameObject);
                Destroy(playerData.EndGameStats.gameObject);
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
            if(CurrentGameState != CURRENT_GAME_STATE.MAIN_MENU)
            {
                Clear();
            }

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
        if (animateTextCoroutine == null)
        animateTextCoroutine = StartCoroutine(IAnimateText(textToAnimate, flickeringSpeed));
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

    #region PLAYER PREFS

    public void SetValueToPlayerPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public void SetValueToPlayerPrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public void SetValueToPlayerPrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public void SetValueToPlayerPrefs(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    public bool GetBooleanFromPlayerPrefs(string key, bool defaultValue = true)
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue)));
    }

    #endregion PLAYER PREFS

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
        IsLoadingScene = true;

        FadeScreenImage(1);

        yield return new WaitUntil(() => !isFading);
        
        messageText.enabled = true;
        messageText.text = "LOADING...";
        loadImageObject.SetActive(CurrentGameState == CURRENT_GAME_STATE.MAIN_MENU ? true : false);

        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
      
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f && !UIManager.Instance.IsLoadImageAnimationRunning)
            {
                if (!isAnimatingText)
                {
                    AnimateText(messageText, flickeringSpeed);
                    messageText.text = "PRESS ANY KEY";
                }

                if (Input.anyKeyDown || !loadImageObject.activeSelf)
                {
                    asyncOperation.allowSceneActivation = true;
                    IsLoadingScene = false;
                }
            }

            yield return null;
        }

        isAnimatingText = false;
        loadImageObject.gameObject.SetActive(false);    

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

        animateTextCoroutine = null;
    }

    #endregion COROUTINES
}
