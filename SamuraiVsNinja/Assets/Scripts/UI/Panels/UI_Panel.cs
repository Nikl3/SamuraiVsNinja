using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UI_Panel : MonoBehaviour
    {
        #region VARIABLES

        private RectTransform rectTransform;

        private Vector2 targetPosition;
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
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponentInChildren<CanvasGroup>();

            targetPosition = rectTransform.anchoredPosition;
            startingPosition = new Vector2(-520, 0);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Open()
        {
            gameObject.SetActive(true);
            canvasGroup.blocksRaycasts = false;
            IsAnimating = true;

            rectTransform.anchoredPosition = startingPosition;

            AudioManager.Instance.PlayUISfx(UI_SFX_TYPE.UI_PANEL_MOVE);

            LeanTween.move(rectTransform, targetPosition, 0.25f)
            .setEaseInBack()
            //.setEaseInBounce()
            //.setEaseInCirc()
            //.setEaseInCubic()
            //.setEaseInElastic()
            //.setEaseInExpo()
            //.setEaseInQuad()
            //.setEaseInQuart()
            //.setEaseInQuint()
            //.setEaseInSine()
            //.setEasePunch()
            //.setEaseShake()
            //.setEaseSpring()
            .setOnComplete(() => 
            {
                canvasGroup.blocksRaycasts = true;
                IsAnimating = false;
            });
        }

        public void Close()
        {
            canvasGroup.blocksRaycasts = false;
            IsAnimating = true;

            rectTransform.anchoredPosition = targetPosition;

            LeanTween.move(rectTransform, startingPosition, 0.25f)
            .setEaseOutBack()
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
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
                IsAnimating = false;
            });
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

