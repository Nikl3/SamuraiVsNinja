using System.Collections;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    public class GameMaster : Singelton<GameMaster>
    {
        #region VARIABLES

        private const int MAX_PLAYER_COUNT = 4;

        public PlayerData[] Players;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            // For testing...

            if(DebugManager.Instance.TestGame)
            {
                LevelManager.Instance.StartLevel();
                return;
            }

            // Refactor...

            var connectedInputDevices = InputManager.Instance.GetConnectedInputDevices();

            var backtrackIndex = 0;

            for(int i = 0; i < connectedInputDevices.Length; i++)
            {
                if(connectedInputDevices[i].InputDevice is Keyboard || connectedInputDevices[i].InputDevice is Mouse)
                {
                    backtrackIndex++;
                    continue;
                }

                Players[i - backtrackIndex].InputDevice = connectedInputDevices[i].InputDevice;
                Players[i - backtrackIndex].InputDeviceName = connectedInputDevices[i].InputDevice.displayName;
            }

            StartCoroutine(IRunGame());
        }

        private void OnValidate()
        {
            for(int i = 0; i < Players.Length; i++)
            {
                Players[i].Initialize();
                //Players[i].Name = $"Player {i + 1}";
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private IEnumerator IRunGame()
        {
            if(DebugManager.Instance.ShowIntro)
            {
                yield return StartCoroutine(VideoManager.Instance.IPlayIntro());
            }

            yield return StartCoroutine(UIManager.Instance.IRunMainMenu());
        }

        #endregion CUSTOM_FUNCTIONS
    }
}