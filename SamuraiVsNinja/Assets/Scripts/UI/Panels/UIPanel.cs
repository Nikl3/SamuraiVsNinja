using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public bool IsOpen = false;
    public UIButton PanelDefaultButton;
    public UIButton LastSelectedButton;

    public virtual void OpenBehaviour()
    {
        if (!IsOpen)
        {
      
            gameObject.SetActive(true);
            IsOpen = true;
            InputManager.Instance.ChangeActiveSelectedObject(PanelDefaultButton.gameObject);
        }
    }

    public virtual void CloseBehaviour()
    {
        if (IsOpen)
        {
            gameObject.SetActive(false);
            IsOpen = false;
        }
    }
}
