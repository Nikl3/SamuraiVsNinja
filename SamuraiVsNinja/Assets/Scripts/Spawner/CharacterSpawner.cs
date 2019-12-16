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

        public void SpawnCharacter(PlayerData playeData)
        {
            var effectPosition = Position + Vector2.down * 5.25f;

            ObjectPoolManager.Instance.Spawn<Resurection_Effect>(effectPosition, Quaternion.identity);

            var newCharacter = ObjectPoolManager.Instance.Spawn<CharacterEngine>(Position, Quaternion.identity);
            newCharacter.SetOwner(playeData);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

