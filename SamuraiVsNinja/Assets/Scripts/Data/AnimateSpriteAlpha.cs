using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class AnimateSpriteAlpha
    {
        [HideInInspector]
        public string Name = "New Sprite Alpha Value To Aniamte...";
        public GameObject SpriteAlphaToAnimateGameObject;
        [Range(0, 1)] public float TargetAlpha;
        [Range(0, 10)] public float TransitionDuration;
        public LeanTweenType LoopType;

        private int animationID;

        public void Play()
        {
            animationID = LeanTween.alpha(
                SpriteAlphaToAnimateGameObject,
                TargetAlpha,
                TransitionDuration)
                .setLoopType(LoopType).id;
        }

        public void Cancel()
        {
            LeanTween.cancel(animationID);
        }
    }
}