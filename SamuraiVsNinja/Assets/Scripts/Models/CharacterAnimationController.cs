using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class CharacterAnimationController : MonoBehaviour
    {
        #region VARIABLES

        public CHARACTER_ANIMATION_TYPE CurrentlyRunningAnimationClipType;
        private Coroutine currentlyPlayingAnimation;

        public AnimationClip[] CharacterAniamtionClips;

        private SpriteRenderer spriteRenderer;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();    
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void PlayAnimation(CHARACTER_ANIMATION_TYPE aniamtionType)
        {
            var animationClip = GetAnimationClip(aniamtionType);

            if(currentlyPlayingAnimation != null)
            {
                StopCoroutine(currentlyPlayingAnimation);
                currentlyPlayingAnimation = null;
            }

            currentlyPlayingAnimation = StartCoroutine(IPlayAnimation(animationClip));

            CurrentlyRunningAnimationClipType = aniamtionType;
        }

        private AnimationClip GetAnimationClip(CHARACTER_ANIMATION_TYPE animationType)
        {
            for(int i = 0; i < CharacterAniamtionClips.Length; i++)
            {
                if(CharacterAniamtionClips[i].AnimationType == animationType)
                {
                    return CharacterAniamtionClips[i];
                }
            }

            Debug.LogError($"No animation clip found with '{animationType}' animation type!");
            return null;
        }

        private IEnumerator IPlayAnimation(AnimationClip animationClip)
        {
            var animationRate = new WaitForSeconds(animationClip.AniamtionRate);
            var animationSprites = animationClip.AnimationSprites;

            do
            {
                for(int i = 0; i < animationSprites.Length; i++)
                {
                    spriteRenderer.sprite = animationSprites[i];
                    yield return animationRate;
                } 
                
            } while(animationClip.IsLooping);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}