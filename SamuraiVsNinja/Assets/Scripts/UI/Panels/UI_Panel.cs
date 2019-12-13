using UnityEngine;
using UnityEngine.Events;

namespace Sweet_And_Salty_Studios
{
    public class UI_Panel : MonoBehaviour
    {
        #region VARIABLES

        public UnityEvent OnOpen;
        public UnityEvent OnClose;

        public Vector2 StartPosition;
        public Vector2 TargetPosition;

        private RectTransform rectTransform;

        private CanvasGroup canvasGroup;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponentInChildren<CanvasGroup>();

            // rectTransform.position = StartPosition;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Open()
        {
            gameObject.SetActive(true);

            //canvasGroup.blocksRaycasts = false;

            //LeanTween.move(rectTransform, TargetPosition, 0.1f)
            //.setEaseInBounce()
            //.setFrom(StartPosition).setOnComplete(() =>
            //{
            //    canvasGroup.blocksRaycasts = true;
            //});               
        }

        public void Close()
        {
            //LeanTween.move(rectTransform, TargetPosition, 0.1f)
            //.setFrom(StartPosition)
            //.setEaseOutBounce()
            //.setOnComplete(() =>
            //{
            //    gameObject.SetActive(false);
            //});

            gameObject.SetActive(false);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

