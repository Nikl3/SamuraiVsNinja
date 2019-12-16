using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class CharacterSpawner : Spawner
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region CUSTOM_FUNCTIONS

        public override void Spawn()
        {
            var effectPosition = new Vector2(Position.x, -5.25f);

            ObjectPoolManager.Instance.Spawn<Resurection_Effect>(effectPosition, Quaternion.identity);
            ObjectPoolManager.Instance.Spawn<CharacterEngine>(Position, Quaternion.identity);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

