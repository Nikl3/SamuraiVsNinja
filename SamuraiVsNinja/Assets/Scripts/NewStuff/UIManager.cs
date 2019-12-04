using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UIManager : Singelton<UIManager>
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public IEnumerator IRunMainMenu()
        {
           

            yield return new WaitForSeconds(1);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

