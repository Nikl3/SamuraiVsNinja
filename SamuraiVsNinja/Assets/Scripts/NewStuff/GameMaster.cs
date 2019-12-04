﻿using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class GameMaster : MonoBehaviour
    {
        #region VARIABLES

    

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
            yield return StartCoroutine(VideoManager.Instance.IPlayIntro());

            yield return StartCoroutine(UIManager.Instance.IRunMainMenu());
        }

        #endregion CUSTOM_FUNCTIONS
    }

}