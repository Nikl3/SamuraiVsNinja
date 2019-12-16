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

        public void SpawnOnigiri()
        {
            ObjectPoolManager.Instance.Spawn<Onigiri>(Position, Quaternion.identity);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

