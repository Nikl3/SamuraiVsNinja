using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class Player
    {
        [HideInInspector] public string Name;

        public int ID
        {
            get;
            private set;
        }

        public string InputDeviceName;
        public InputDevice InputDevice;

        public Player(int id)
        {
            Name = $"Player: {id}";

            ID = id;
        }
    }

    public class GameMaster : Singelton<GameMaster>
    {
        #region VARIABLES

        private const int MAX_PLAYER_COUNT = 4;

        public Player[] Players;

        public bool ShowIntro;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            StartCoroutine(IRunGame());

            CreatePlayers();
        }



        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void CreatePlayers()
        {
            Players = new Player[MAX_PLAYER_COUNT];

            for(int i = 0; i < MAX_PLAYER_COUNT; i++)
            {
                Players[i] = new Player(i + 1);
            }
        }

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