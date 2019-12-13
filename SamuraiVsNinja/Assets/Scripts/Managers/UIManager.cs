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

        private IEnumerator IFoo()
        {
            var foo = TitleImage.GetComponent<RectTransform>();

            LeanTween.moveY(
                foo,
                TargetPosition_Y,
                0.6f)
                .setFrom(StartPosition_Y)
                .setEaseOutBounce();

            var foo2 = TitleAnimationImage.GetComponent<RectTransform>();

            LeanTween.play(
                foo2,
                TitleCharacterAnimationSprites)
                .setSpeed(10);

            yield return new WaitWhile(() => LeanTween.isTweening(foo.gameObject) && LeanTween.isTweening(foo2));
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

            yield return IFoo();

            if(StartingPanel)
            {
                SwitchUIPanel(StartingPanel);
            }

            yield return null;
        }

#endregion CUSTOM_FUNCTIONS
    }
}

