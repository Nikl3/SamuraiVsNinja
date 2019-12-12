using System.Collections;
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

        private void OnEnable()
        {
            LeanTween.moveY(
                TitleImage.GetComponent<RectTransform>(),
                TargetPosition_Y, 
                0.6f)
                .setFrom(StartPosition_Y)
                .setEaseOutBounce();

            LeanTween.play(TitleAnimationImage.GetComponent<RectTransform>(), TitleCharacterAnimationSprites).setSpeed(10);
        }

        private void OnDisable()
        {

        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void SwitchUIPanel(UI_Panel panel)
        {
            if(currentPanel)
            {
                currentPanel.Close();
            }

            currentPanel = panel;

            currentPanel.Open();
        }

        public IEnumerator IRunMainMenu()
        {
            if(StartingPanel)
            {
                SwitchUIPanel(StartingPanel);
            }

            var panels = GetComponentsInChildren<UI_Panel>(true);

            for(int i = 0; i < panels.Length; i++)
            {
                if(panels[i] != StartingPanel)
                {
                    panels[i].Close();
                }
            }

            yield return new WaitForSeconds(1);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

