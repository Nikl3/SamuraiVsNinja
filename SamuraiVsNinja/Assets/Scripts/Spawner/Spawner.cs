using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public abstract class Spawner
    {
        #region VARIABLES

        public Vector2 Position;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region CUSTOM_FUNCTIONS

        public abstract void Spawn();

        #endregion CUSTOM_FUNCTIONS
    }
}

