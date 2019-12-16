using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Onigiri : MonoBehaviour
    {
        #region VARIABLES

        public Effect ItemCollectEffectPrefab;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            Instantiate(ItemCollectEffectPrefab, transform.position, Quaternion.identity, null);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}

