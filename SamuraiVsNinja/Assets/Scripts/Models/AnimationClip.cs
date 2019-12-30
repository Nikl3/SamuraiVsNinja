using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class AnimationClip
    {
        #region VARIABLES

        public CHARACTER_ANIMATION_TYPE AnimationType;
        public Sprite[] AnimationSprites;
        public bool IsLooping;
        public float AniamtionRate = 0.2f;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}

