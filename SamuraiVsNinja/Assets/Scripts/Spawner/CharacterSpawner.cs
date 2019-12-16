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
            UnityEngine.Object.Instantiate(LevelManager.Instance.RespawnEffectPrefab, new Vector2(Position.x, -5.25f), Quaternion.identity);
            UnityEngine.Object.Instantiate(LevelManager.Instance.CharacterEnginePrefab, Position, Quaternion.identity);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

