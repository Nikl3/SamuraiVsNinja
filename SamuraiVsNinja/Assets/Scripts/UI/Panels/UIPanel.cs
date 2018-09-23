using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public GameObject DefaultSelectedObject;
    public GameObject LastSelectedObject;
    public bool IsOpen { get; private set; }

    public virtual void OpenBehaviour()
    {
        if (!IsOpen)
        {   
            gameObject.SetActive(true);
            IsOpen = true;
            if (LastSelectedObject == null)
                LastSelectedObject = DefaultSelectedObject;
        }
    }

    public virtual void Update()
    {
        InputManager.Instance.FocusToButton(LastSelectedObject);
    }

    public virtual void CloseBehaviour()
    {
        if (IsOpen)
        {
            gameObject.SetActive(false);
            IsOpen = false;
    
            LastSelectedObject = InputManager.Instance.PreviousSelectedObject;
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
