using Fabric;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField]
    protected GameObject defaultSelectedObject;
    [SerializeField]
    protected GameObject lastSelectedObject;

    public bool IsOpen
    {
        get;
        private set;
    }

    public virtual void OpenBehaviour()
    {
        if (!IsOpen)
        {   
            gameObject.SetActive(true);
            IsOpen = true;
            InputManager.Instance.ChangeActiveSelectedObject(lastSelectedObject ?? defaultSelectedObject);           
        }
    }

    public virtual void Update()
    {
        InputManager.Instance.FocusToButton(lastSelectedObject);
    }

    public virtual void CloseBehaviour()
    {
        if (IsOpen)
        {
            gameObject.SetActive(false);
            IsOpen = false;
            lastSelectedObject = InputManager.Instance.CurrentSelectedObject;
            EventManager.Instance.PostEvent("UI_PanelOpen");
        }
    }

    public void OptionsButton()
    {
        UIManager.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }

    public void RestartGameButton()
    {
        GameMaster.Instance.LoadScene(1);
    }

    public virtual void BackButton()
    {
        
    }
}
