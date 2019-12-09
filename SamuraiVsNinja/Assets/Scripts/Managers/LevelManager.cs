using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class LevelManager : Singelton<LevelManager>
    {
        #region VARIABLES

        [Space]
        [Header("Level settings")]
        [Range(2, 4)]
        public int PlayerCount = 2;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void StartLevel()
        {
         
        }

        #endregion CUSTOM_FUNCTIONS
    }
}