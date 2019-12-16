using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class PlayerData
    {
        [HideInInspector] public string Name;

        public Color IndicatorColor;
        public string InputDeviceName;
        public InputDevice InputDevice;

        public int ID
        {
            get;
            private set;
        }

        public PlayerData(int id)
        {
            Name = $"Player: {id}";

            ID = id;
        }
    }

    public class GameMaster : Singelton<GameMaster>
    {
        #region VARIABLES

        private const int MAX_PLAYER_COUNT = 4;

        public PlayerData[] Players;

        public bool ShowIntro;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            StartCoroutine(IRunGame());
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private IEnumerator IRunGame()
        {
            UIManager.Instance.Fade(0, 1);

            if(ShowIntro)
            {
                yield return StartCoroutine(VideoManager.Instance.IPlayIntro());
            }

            yield return StartCoroutine(UIManager.Instance.IRunMainMenu());
        }    

        #endregion CUSTOM_FUNCTIONS
    }
}