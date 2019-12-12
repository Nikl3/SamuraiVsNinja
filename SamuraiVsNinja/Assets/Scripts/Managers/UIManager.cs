using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        #region VARIABLES

        public UI_Panel StartingPanel;
        public UI_Panel InGamePanel;
        public UI_Panel VictoryPanel;

        private UI_Panel currentPanel;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

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

