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

            LeanTween.move(rectTransform, targetPosition, 0.25f)
            .setEaseInElastic()
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
            .setEaseInOutElastic()
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
                IsAnimating = false;
            });
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

