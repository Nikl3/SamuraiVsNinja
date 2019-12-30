using UnityEngine;
using UnityEditor;

namespace Sweet_And_Salty_Studios
{
    public class DebugManager : Singelton<DebugManager>
    {
        #region VARIABLES

        [Space]
        [Header("Test Settings")]
        public bool ShowIntro;
        public bool TestGame;

        [Space]
        [Header("Show / Hide")]
        public bool ShowCharacterRaycasts;
        public Color CharacterRaycastsColor;
        [Space]
        public bool ShowCameraEncapsulateBounds;
        public Color CameraEncapsulateOuterBoundsColor;
        public Color CameraEncapsulateInnerBoundsColor;
        [Space]
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
                    Gizmos.color = CameraEncapsulateOuterBoundsColor;

                    var bounds = LevelManager.Instance.GameCamera.CurrentEncapsulateBounds;

                    Gizmos.DrawWireCube(bounds.center, bounds.extents * 2f);

                    Gizmos.color = CameraEncapsulateInnerBoundsColor;

                    Gizmos.DrawCube(bounds.center, bounds.extents * 2);
                }
            }
            else
            {            
                if(LevelManager.Instance.ItemSpawners != null)
                {
                    var spawnPositions = LevelManager.Instance.ItemSpawners;

                    for(int i = 0; i < spawnPositions.Length; i++)
                    {
                        Debug.DrawLine(spawnPositions[i].Position + Vector2.left * 1f,
                            spawnPositions[i].Position + Vector2.right * 1f,
                            OnigiriSpawnPositionColor);

                        Debug.DrawLine(spawnPositions[i].Position + Vector2.down * 1f,
                         spawnPositions[i].Position + Vector2.up * 1f,
                         OnigiriSpawnPositionColor);
                    }
                }

                if(LevelManager.Instance.CharacterSpawners != null)
                {
                    var characterSpawners = LevelManager.Instance.CharacterSpawners;

                    for(int i = 0; i < characterSpawners.Length; i++)
                    {
                        Debug.DrawLine(characterSpawners[i].Position + Vector2.left * 1f,
                            characterSpawners[i].Position + Vector2.right * 1f,
                            CharacterSpawnPositionColor);

                        Debug.DrawLine(characterSpawners[i].Position + Vector2.down * 1f,
                         characterSpawners[i].Position + Vector2.up * 1f,
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

