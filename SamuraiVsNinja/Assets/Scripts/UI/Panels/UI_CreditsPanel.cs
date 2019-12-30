using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UI_CreditsPanel : UI_Panel
    {
        #region VARIABLES

        [Space]
        [Header("Credits Animation Properties")]
        public RectTransform CreditsTextRectTransform;
        [Range(1f, 40f)] public float RollDuration = 10f;
        private Vector2 creditsStartPosition;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected override void Initialize()
        {
            base.Initialize();

            creditsStartPosition = CreditsTextRectTransform.anchoredPosition;
        }

        public override void Open()
        {
            base.Open();

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.CREDITS, true);

            LeanTween.moveY(CreditsTextRectTransform, 975, RollDuration)
            .setLoopClamp();         
        }

        public override void Close()
        {
            base.Close();

            AudioManager.Instance.PlayMusicTrack(MUSIC_TRACK_TYPE.MENU, true);

            LeanTween.cancel(CreditsTextRectTransform);
            CreditsTextRectTransform.anchoredPosition = creditsStartPosition;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

