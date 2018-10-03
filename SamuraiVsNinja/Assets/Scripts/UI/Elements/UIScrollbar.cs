using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollbar : Scrollbar
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        InputManager.Instance.ChangeActiveSelectedObject(gameObject);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        InputManager.Instance.ChangeActiveSelectedObject(null);
    }
}
