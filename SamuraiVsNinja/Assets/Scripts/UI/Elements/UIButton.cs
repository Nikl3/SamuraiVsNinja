using Fabric;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button
{
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
		EventManager.Instance.PostEvent("UI_Hover");
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		EventManager.Instance.PostEvent("UI_Press");
	}
}
