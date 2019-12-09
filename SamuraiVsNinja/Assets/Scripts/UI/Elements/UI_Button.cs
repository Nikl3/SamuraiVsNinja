using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sweet_And_Salty_Studios
{
    public class UI_Button : Button
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

      

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnSelect(eventData);

            LeanTween.scale(targetGraphic.gameObject, Vector2.one * 1.1f, 0.25f);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnDeselect(eventData);

            LeanTween.scale(targetGraphic.gameObject, Vector2.one, 0.25f);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}