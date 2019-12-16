using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class LevelManager : Singelton<LevelManager>
    {
        #region VARIABLES

        [Space]
        [Header("Spawners")]
        public CharacterSpawner[] CharacterSpawners;
        public OnigiriSpawner[] ItemSpawners;

        [Space]
        [Header("Prefabs")]
        public Onigiri OnigiriPrefab;

        private bool isPlaying;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void StartLevel()
        {
            StartCoroutine(IStartLevel());
        }

        public Vector2 GetNearestCharacterSpawnPosition(Vector2 currentPosition)
        {
            var shortestDistance = Mathf.Infinity;
            var currentResult = 0f;
            var nearestSpawnPoint = Vector2.zero;

            for(int i = 0; i < CharacterSpawners.Length; i++)
            {
                currentResult = Vector2.Distance(currentPosition, CharacterSpawners[i].Position);

                if(currentResult < shortestDistance)
                {
                    shortestDistance = currentResult;
                    nearestSpawnPoint = CharacterSpawners[i].Position;
                }
            }

            return nearestSpawnPoint;
        }

        private void SpawnCharacters()
        {
            var currentPlayers = GameMaster.Instance.Players;

            for(int i = 0; i < currentPlayers.Length; i++)
            {
                CharacterSpawners[i].SpawnCharacter(currentPlayers[i]);            
            }
        }

        private void StartSpawnOnigiris()
        {
            StartCoroutine(IStartSpawnOnigiris());
        }

        private IEnumerator IStartLevel()
        {
            isPlaying = true;

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.GAME);

            SpawnCharacters();

            StartSpawnOnigiris();

            yield return null;
        }

        private IEnumerator IStartSpawnOnigiris()
        {
            var onigiriSpawnTime = Random.Range(2, 6);
            var randomOnigiriPositionIndex = 0;
            while(isPlaying)
            {
                yield return new WaitForSeconds(onigiriSpawnTime);

                randomOnigiriPositionIndex = Random.Range(0, ItemSpawners.Length - 1);

                if(Physics2D.BoxCast(ItemSpawners[randomOnigiriPositionIndex].Position, Vector2.one, 0, Vector2.up) == false)
                {
                    Instantiate(
                     OnigiriPrefab,
                     ItemSpawners[randomOnigiriPositionIndex].Position,
                     Quaternion.identity
                 );
                }        

                onigiriSpawnTime = Random.Range(2, 6);

                yield return null;
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}