using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public UIButton PanelDefaultButton;
    public bool IsOpen { get; private set; }

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
            InputManager.Instance.ChangeActiveSelectedObject(null);
        }
    }
}
