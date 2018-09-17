using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (gameObject != InputManager.Instance.CurrentSeletedObject)
        {
            print("true");
            InputManager.Instance.ChangeActiveSelectedObject(gameObject);
        }
        else
        {
            print("false");
            eventData.selectedObject = null;
        }
    }

    //public override void OnMove(AxisEventData eventData)
    //{
    //    base.OnMove(eventData);
    //    eventData.selectedObject = null;
    //    print("FOO");
    //}
}
