using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        #region VARIABLES

        [Space]
        [Header("UI Panels")]
        public UI_Panel StartingPanel;
        public UI_Panel InGamePanel;
        public UI_Panel VictoryPanel;

        private UI_Panel currentPanel;
        private readonly float panelSwitchDelay = 0.25f;

        [Space]
        [Header("Join fields")]
        public JoinPlayerField[] JoinPlayerFields = new JoinPlayerField[4];

        [Space]
        [Header("Fade Screen Animation")]
        public Image FadeImage;
        public float FadeDuration = 1f;

        [Space]
        [Header("Title Animation")]
        public Image TitleImage;
        public float StartPosition_Y;
        public float TargetPosition_Y;
       
        [Space]
        [Header("Title Character Animation")]
        public Sprite[] TitleCharacterAnimationSprites;
        public Image TitleAnimationImage;

        [Space]
        [Header("Others")]
        public GameObject MenuBackgroundContainer;
        public GameObject FakeLoadImageParent;
        public Image FakeLoadScreenImage;
        public Sprite[] FakeLoadScreenSprites;
        public TextMeshProUGUI PressToContinueText;
        public float FakeLoadImageDuration = 4f;
        public float MenuStartDelay = 1f;

        private bool isFading;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            FakeLoadImageParent.SetActive(false);
            FadeImage.gameObject.SetActive(true);
        }

        private void Start()
        {
            var panels = GetComponentsInChildren<UI_Panel>(true);

            for(int i = 0; i < panels.Length; i++)
            {
                panels[i].gameObject.SetActive(false);          
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void SwitchUIPanel(UI_Panel panel)
        {
            if(panel == null)
            {
                return;
            }

            StartCoroutine(ISwitchPanel(panel));
        }

        private IEnumerator ISwitchPanel(UI_Panel panel)
        {
            if(currentPanel)
            {
                currentPanel.Close();
                yield return new WaitWhile(() => currentPanel.IsAnimating);
            }

            currentPanel = panel;
            yield return new WaitForSeconds(panelSwitchDelay);

            currentPanel.Open();
            yield return new WaitWhile(() => currentPanel.IsAnimating);
        }

        public IEnumerator IFade(float targetFillAmount, float fadeDuration)
        {
            yield return new WaitWhile(() => isFading);

            FadeImage.gameObject.SetActive(true);

            var startLerpTime = Time.unscaledTime;
            var timeSinceStarted = Time.unscaledTime - startLerpTime;
            var percentToComplete = timeSinceStarted / fadeDuration;

            if(FadeImage.fillAmount != targetFillAmount)
            {
                isFading = true;

                while(percentToComplete <= 1f)
                {
                    timeSinceStarted = Time.unscaledTime - startLerpTime;
                    percentToComplete = timeSinceStarted / fadeDuration;

                    var currentVolume = Mathf.Lerp(FadeImage.fillAmount, targetFillAmount, percentToComplete);
                    FadeImage.fillAmount = currentVolume;

                    yield return null;
                }

                isFading = false;
            }

            FadeImage.gameObject.SetActive(false);
        }

        private IEnumerator IAnimateMenuTitle()
        {
            var titleImageRectTransform = TitleImage.GetComponent<RectTransform>();

            LeanTween.moveY(
                titleImageRectTransform,
                TargetPosition_Y,
                0.6f)
                .setFrom(StartPosition_Y)
                .setEaseOutBounce();

            // Should ask if we are int the game...
            yield return new WaitWhile(() => LeanTween.isTweening(titleImageRectTransform.gameObject));
        }

        private IEnumerator IAnimateMenuCharacters()
        {
            var titleAniamtionImageRectTransform = TitleAnimationImage.GetComponent<RectTransform>();

            LeanTween.play(
                titleAniamtionImageRectTransform,
                TitleCharacterAnimationSprites)
                .setSpeed(10);

            yield return null;
        }

        public IEnumerator IRunMainMenu()
        {
            FadeImage.fillAmount = 1f;
            var titleImageRectTransform = TitleImage.GetComponent<RectTransform>();
            titleImageRectTransform.anchoredPosition = Vector2.up * StartPosition_Y;

            yield return IAnimateMenuCharacters();

            yield return StartCoroutine(IFade(0, FadeDuration));

            yield return new WaitForSeconds(MenuStartDelay);

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.MENU, false);

            yield return IAnimateMenuTitle();

            if(StartingPanel)
            {
                SwitchUIPanel(StartingPanel);
            }

            yield return null;
        }

        public IEnumerator IFadeToGameScreen()
        {
            if(DebugManager.Instance.TestGame)
            {
                yield return IFade(1, FadeDuration);

                MenuBackgroundContainer.SetActive(false);
                AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.GAME, false);

                yield return IFade(0, FadeDuration);

                yield break;
            }

            yield return IFade(1, FadeDuration);

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.PAUSED, false);

            var randomSprite = FakeLoadScreenSprites[UnityEngine.Random.Range(0, FakeLoadScreenSprites.Length)];
            FakeLoadScreenImage.sprite = randomSprite;

            FakeLoadImageParent.SetActive(true);

            LeanTween.color(FakeLoadScreenImage.gameObject, Color.white, FadeDuration)
            .setFromColor(Color.clear)
            .setOnComplete(() =>
            {
               
                MenuBackgroundContainer.SetActive(false);
            });

            yield return new WaitWhile(() => LeanTween.isTweening(FakeLoadScreenImage.gameObject));

            yield return new WaitForSeconds(FakeLoadImageDuration);

            // Show "Press any key" text in UI?...
            LeanTween.scale(PressToContinueText.gameObject, Vector2.one * 1.025f, 0.8f)
            .setOvershoot(0.1f)
            .setLoopPingPong();

            LeanTween.alpha(PressToContinueText.gameObject, 0, 0.45f)
            .setFromColor(PressToContinueText.color)
            .setLoopPingPong();


            yield return new WaitUntil(() => InputManager.Instance.StartPressed);

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.GAME, false);

            FakeLoadImageParent.SetActive(false);

            yield return IFade(0, FadeDuration);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

