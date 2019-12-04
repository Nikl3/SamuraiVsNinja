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
            //EventManager.Instance.PostEvent("UI_PanelOpen");
            Debug.LogError("Play UI_PanelOpen here!");
        }
    }

    public void OptionsButton()
    {
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }

    public void RestartGameButton()
    {
        GameManager.Instance.LoadSceneAsync(2);
    }

    public virtual void BackButton()
    {
        
    }
}
