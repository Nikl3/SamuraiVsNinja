using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UI_Panel : MonoBehaviour
    {
        #region VARIABLES

        private RectTransform rectTransform;

        private Vector2 endPosition;
        private Vector2 startingPosition;

        private CanvasGroup canvasGroup;

        #endregion VARIABLES

        #region PROPERTIES

        public bool IsAnimating
        {
            get;
            private set;
        } = false;

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Initialize();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected virtual void Initialize()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponentInChildren<CanvasGroup>();

            endPosition = rectTransform.anchoredPosition;
            startingPosition = new Vector2(-520, 0);
        }

        private void HandleAnimation(Vector2 targetPosition, LeanTweenType easeType, Action onComplete)
        {
            LeanTween.move(rectTransform, targetPosition, 0.25f)
            .setEase(easeType)
           //.setEaseOutBack()
           //.setEaseOutBounce()
           //.setEaseOutCirc()
           //.setEaseOutCubic()
           //.setEaseOutElastic()
           //.setEaseInOutExpo()
           //.setEaseOutQuad()
           //.setEaseOutQuart()
           //.setEaseOutQuint()
           //.setEaseOutSine()
           //.setEasePunch()
           //.setEaseShake()
           //.setEaseSpring()
           .setOnComplete(onComplete);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            canvasGroup.blocksRaycasts = false;
            IsAnimating = true;

            rectTransform.anchoredPosition = startingPosition;

            AudioManager.Instance.PlayUISfx(UI_SFX_TYPE.UI_PANEL_MOVE);

            HandleAnimation(endPosition, LeanTweenType.easeInBack, () =>
            {
                canvasGroup.blocksRaycasts = true;
                IsAnimating = false;
            });
        }

        public virtual void Close()
        {
            canvasGroup.blocksRaycasts = false;
            IsAnimating = true;

            rectTransform.anchoredPosition = endPosition;

            HandleAnimation(startingPosition, LeanTweenType.easeOutBack, () =>
            {
                gameObject.SetActive(false);
                IsAnimating = false;
            });
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

