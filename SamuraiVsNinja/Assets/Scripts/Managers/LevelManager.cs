using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class LevelManager : Singelton<LevelManager>
    {
        #region VARIABLES

        public float StartSpawningPlayerDelayFromStart = 2;

        private Coroutine iStartLevel;

        [Space]
        [Tooltip("Add 'GameObject' from scene to animate it's sprites alpha value." +
            " The root game object or it's child should have sprite renderer component." +
            " One sprite renderer per this array element.")]
        [Header("Animate GameObjects Sprite Alpha")]
        public AnimateSpriteAlpha[] SpriteAlphaValuesToAnimate;

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

        public CameraEngine GameCamera
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            GameCamera = FindObjectOfType<CameraEngine>();
        }

        private void OnValidate()
        {
            if(SpriteAlphaValuesToAnimate == null || SpriteAlphaValuesToAnimate.Length == 0)
            {
                return;
            }

            for(int i = 0; i < SpriteAlphaValuesToAnimate.Length; i++)
            {
                if(SpriteAlphaValuesToAnimate[i].SpriteAlphaToAnimateGameObject == null)
                {
                    continue;
                }

                SpriteAlphaValuesToAnimate[i].Name = SpriteAlphaValuesToAnimate[i].SpriteAlphaToAnimateGameObject.name;
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void StartAnimateSpriteAlphaValues()
        {
            for(int i = 0; i < SpriteAlphaValuesToAnimate.Length; i++)
            {
                SpriteAlphaValuesToAnimate[i].Play();
            }
        }

        private void SpawnCharacters()
        {
            var currentPlayers = GameMaster.Instance.Players;

            for(int i = 0; i < currentPlayers.Length; i++)
            {
                CharacterSpawners[i].SpawnCharacter(currentPlayers[i]);            
            }
        }

        private float GetRandomValue(float min, float max)
        {
            return Random.Range(min, max);
        }

        private void StartSpawnOnigiris()
        {
            StartCoroutine(IStartSpawnOnigiris());
        }

        private IEnumerator IStartLevel()
        {
            yield return StartCoroutine(UIManager.Instance.IFadeToGameScreen());

            isPlaying = true;

            yield return new WaitForSeconds(StartSpawningPlayerDelayFromStart);

            SpawnCharacters();

            StartSpawnOnigiris();

            StartAnimateSpriteAlphaValues();

            yield return null;
        }

        private IEnumerator IStartSpawnOnigiris()
        {
            var onigiriSpawnTime = GetRandomValue(2, 6);
            var randomOnigiriPositionIndex = 0;
            while(isPlaying)
            {
                yield return new WaitForSeconds(onigiriSpawnTime);

                randomOnigiriPositionIndex = (int)GetRandomValue(0, ItemSpawners.Length - 1);

                if(Physics2D.BoxCast(ItemSpawners[randomOnigiriPositionIndex].Position, Vector2.one, 0, Vector2.up) == false)
                {
                    Instantiate(
                     OnigiriPrefab,
                     ItemSpawners[randomOnigiriPositionIndex].Position,
                     Quaternion.identity
                 );
                }        

                onigiriSpawnTime = GetRandomValue(2, 6);

                yield return null;
            }
        }

        public void StartLevel()
        {         
            if(iStartLevel != null)
            {
                StopCoroutine(iStartLevel);
                iStartLevel = null;
            }

            iStartLevel = StartCoroutine(IStartLevel());
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

        #endregion CUSTOM_FUNCTIONS
    }
}