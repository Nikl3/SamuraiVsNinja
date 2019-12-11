using UnityEngine;
using UnityEditor;

namespace Sweet_And_Salty_Studios
{
    public class DebugManager : Singelton<DebugManager>
    {
        #region VARIABLES

        [Space]
        [Header("Show / Hide")]
        public bool ShowCharacterRaycasts;
        public Color CharacterRaycastsColor;
        public bool ShowCameraEncapsulateBounds;
        public Color CameraEncapsulateBoundsColor;

        public Color CharacterSpawnPositionColor;
        public Color OnigiriSpawnPositionColor;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void OnDrawGizmos()
        {
            if(EditorApplication.isPlaying)
            {
                if(ShowCameraEncapsulateBounds)
                {
                    Gizmos.color = CameraEncapsulateBoundsColor;

                    var bounds = CameraEngine.Instance.CurrentEncapsulateBounds;

                    Gizmos.DrawCube(bounds.center, bounds.extents * 2);
                }
            }
            else
            {
                if(LevelManager.Instance.CharacterSpawnPositions != null)
                {
                    var spawnPositions = LevelManager.Instance.CharacterSpawnPositions;

                    for(int i = 0; i < spawnPositions.Length; i++)
                    {
                        Debug.DrawLine(spawnPositions[i] + Vector2.left * 1f,
                            spawnPositions[i] + Vector2.right * 1f,
                            CharacterSpawnPositionColor);

                        Debug.DrawLine(spawnPositions[i] + Vector2.down * 1f,
                         spawnPositions[i] + Vector2.up * 1f,
                         CharacterSpawnPositionColor);
                    }
                }

                if(LevelManager.Instance.OnigiriSpawnPositions != null)
                {
                    var spawnPositions = LevelManager.Instance.OnigiriSpawnPositions;

                    for(int i = 0; i < spawnPositions.Length; i++)
                    {
                        Debug.DrawLine(spawnPositions[i] + Vector2.left * 1f,
                            spawnPositions[i] + Vector2.right * 1f,
                            OnigiriSpawnPositionColor);

                        Debug.DrawLine(spawnPositions[i] + Vector2.down * 1f,
                         spawnPositions[i] + Vector2.up * 1f,
                         OnigiriSpawnPositionColor);
                    }
                }

                if(LevelManager.Instance.CharacterSpawnPositions != null)
                {
                    var spawnPositions = LevelManager.Instance.CharacterSpawnPositions;

                    for(int i = 0; i < spawnPositions.Length; i++)
                    {
                        Debug.DrawLine(spawnPositions[i] + Vector2.left * 1f,
                            spawnPositions[i] + Vector2.right * 1f,
                            CharacterSpawnPositionColor);

                        Debug.DrawLine(spawnPositions[i] + Vector2.down * 1f,
                         spawnPositions[i] + Vector2.up * 1f,
                         CharacterSpawnPositionColor);
                    }
                }
            }       
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void DrawRay(Vector2 Origin, Vector2 direction)
        {
            if(ShowCharacterRaycasts == false)
            {
                return;
            }

            Debug.DrawRay(Origin, direction, CharacterRaycastsColor);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

