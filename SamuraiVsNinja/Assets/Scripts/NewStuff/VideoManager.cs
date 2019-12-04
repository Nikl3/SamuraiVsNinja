using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Sweet_And_Salty_Studios
{
    public class VideoManager : Singelton<VideoManager>
    {
        #region VARIABLES

        private VideoPlayer VideoPlayer;

        [Range(0, 10)]
        public float IntroStartDelay = 3;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            VideoPlayer = GetComponentInChildren<VideoPlayer>();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public IEnumerator IPlayIntro()
        {
            if(VideoPlayer == null)
            {
                Debug.LogError("Video player is NULL...");
                yield break;
            }

            yield return new WaitForSeconds(IntroStartDelay);

            VideoPlayer.Play();

            yield return new WaitUntil(() => VideoPlayer.isPlaying == false || InputManager.Instance.IsAnyKeyDown);

            VideoPlayer.Stop();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

