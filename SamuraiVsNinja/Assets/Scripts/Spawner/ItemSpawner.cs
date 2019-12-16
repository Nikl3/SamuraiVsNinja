using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class OnigiriSpawner : Spawner
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region CUSTOM_FUNCTIONS

        public override void Spawn()
        {
            UnityEngine.Object.Instantiate(LevelManager.Instance.OnigiriPrefab, Position, Quaternion.identity);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

