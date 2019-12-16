using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        #region VARIABLES

        public UI_Panel StartingPanel;
        public UI_Panel InGamePanel;
        public UI_Panel VictoryPanel;

        private UI_Panel currentPanel;

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
        [Header("Title Character Animation")]
        public Sprite[] TitleCharacterAnimationSprites;
        public Image TitleAnimationImage;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            
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
            if(currentPanel)
            {
                currentPanel.Close();
            }

            currentPanel = panel;

            currentPanel.Open();
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

            yield return new WaitWhile(() => LeanTween.isTweening(titleImageRectTransform.gameObject) && LeanTween.isTweening(titleAniamtionImageRectTransform));
        }

        public IEnumerator IRunMainMenu()
        {
            var panels = GetComponentsInChildren<UI_Panel>(true);

            for(int i = 0; i < panels.Length; i++)
            {
                if(panels[i] != StartingPanel)
                {
                    panels[i].gameObject.SetActive(false);
                }
            }

            yield return IAnimateMenu();

            if(StartingPanel)
            {
                SwitchUIPanel(StartingPanel);
            }

            yield return null;
        }

#endregion CUSTOM_FUNCTIONS
    }
}

