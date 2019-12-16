using System.Collections;
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

        [Space]
        [Header("Title Animation")]
        public Image TitleImage;
        public float StartPosition_Y;
        public float TargetPosition_Y;

        [Space]
        [Header("Credits Animation")]
        public RectTransform RectTransformToAnimate;
        [Range(1f, 40f)] public float RollDuration = 10f;
        private Vector2 creditsStartPosition;

        [Space]
        [Header("Title Character Animation")]
        public Sprite[] TitleCharacterAnimationSprites;
        public Image TitleAnimationImage;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            FadeImage.gameObject.SetActive(true);
        }

        private void Start()
        {
            var panels = GetComponentsInChildren<UI_Panel>(true);

            for(int i = 0; i < panels.Length; i++)
            {
                panels[i].gameObject.SetActive(false);          
            }

            creditsStartPosition = RectTransformToAnimate.anchoredPosition;
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

        public void Fade(float targetAlpha, float fromAlpha)
        {
            LeanTween.color(FadeImage.gameObject, targetAlpha > 0 ? Color.black : Color.clear, 0.4f);
        }

        private IEnumerator IAnimateMenu()
        {
            var titleImageRectTransform = TitleImage.GetComponent<RectTransform>();

            LeanTween.moveY(
                titleImageRectTransform,
                TargetPosition_Y,
                0.6f)
                .setFrom(StartPosition_Y)
                .setEaseOutBounce();

            var titleAniamtionImageRectTransform = TitleAnimationImage.GetComponent<RectTransform>();

            LeanTween.play(
                titleAniamtionImageRectTransform,
                TitleCharacterAnimationSprites)
                .setSpeed(10);

            // Should ask if we are int the game...
            yield return new WaitWhile(() => LeanTween.isTweening(titleImageRectTransform.gameObject) && LeanTween.isTweening(titleAniamtionImageRectTransform));
        }

        public IEnumerator IRunMainMenu()
        {
            yield return IAnimateMenu();

            FadeImage.gameObject.SetActive(false);

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.MENU);

            if(StartingPanel)
            {
                SwitchUIPanel(StartingPanel);
            }

            yield return null;
        }

        public void PlayCredits()
        {
            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.PAUSED);

            LeanTween.moveY(RectTransformToAnimate, 975, RollDuration)
            .setLoopClamp();
        }

        public void CancelCredits()
        {
            LeanTween.cancel(RectTransformToAnimate);
            RectTransformToAnimate.anchoredPosition = creditsStartPosition;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

