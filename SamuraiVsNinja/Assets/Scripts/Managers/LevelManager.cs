using System.Collections;
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
        public Vector2[] CharacterSpawnPositions = new Vector2[4];
        public Vector2[] OnigiriSpawnPositions = new Vector2[4];

        public CharacterEngine CharacterPrefab;
        public Effect ResurectionEffectPrefab;
        public Onigiri OnigiriPrefab;

        private bool isPlaying;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            StartLevel();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void StartLevel()
        {
            StartCoroutine(IStartLevel());
        }

        public Vector2 GetNearestSpawnPoint(Vector2 currentPosition)
        {
            var shortestDistance = Mathf.Infinity;
            var currentResult = 0f;
            var nearestSpawnPoint = Vector2.zero;

            for(int i = 0; i < CharacterSpawnPositions.Length; i++)
            {
                currentResult = Vector2.Distance(currentPosition, CharacterSpawnPositions[i]);

                if(currentResult < shortestDistance)
                {
                    shortestDistance = currentResult;
                    nearestSpawnPoint = CharacterSpawnPositions[i];
                }
            }

            return nearestSpawnPoint;
        }

        private void CreatePlayers()
        {
            for(int i = 0; i < PlayerCount; i++)
            {
                Instantiate(ResurectionEffectPrefab, CharacterSpawnPositions[i], Quaternion.identity);
                Instantiate(CharacterPrefab, CharacterSpawnPositions[i] + Vector2.up * 4 , Quaternion.identity);
            }
        }

        private void StartSpawnOnigiris()
        {
            StartCoroutine(IStartSpawnOnigiris());
        }

        private IEnumerator IStartLevel()
        {
            isPlaying = true;

            CreatePlayers();

            StartSpawnOnigiris();

            yield return null;
        }

        private IEnumerator IStartSpawnOnigiris()
        {
            var onigiriSpawnTime = Random.Range(2, 6);

            while(isPlaying)
            {
                yield return new WaitForSeconds(onigiriSpawnTime);

                Instantiate(
                        OnigiriPrefab,
                        OnigiriSpawnPositions[Random.Range(0, OnigiriSpawnPositions.Length - 1)],
                        Quaternion.identity
                    );

                onigiriSpawnTime = Random.Range(2, 6);

                yield return null;
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}