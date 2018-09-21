using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    private bool isOpen = false;
    public UIButton PanelDefaultButton;
 
    public virtual void OpenBehaviour()
    {
        if (!isOpen)
        {   
            gameObject.SetActive(true);
            isOpen = true;
            InputManager.Instance.ChangeActiveSelectedObject(PanelDefaultButton.gameObject);
        }
    }

    public virtual void CloseBehaviour()
    {
        if (isOpen)
        {
            gameObject.SetActive(false);
            isOpen = false;
            InputManager.Instance.ChangeActiveSelectedObject(null);
        }
    }
}
