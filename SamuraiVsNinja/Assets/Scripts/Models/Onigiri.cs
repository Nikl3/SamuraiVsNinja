﻿using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Onigiri : MonoBehaviour
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            ObjectPoolManager.Instance.Despawn(this);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}

