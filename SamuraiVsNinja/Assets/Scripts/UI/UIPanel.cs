using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public UIButton panelDefaultButton;

    public void SetActive(bool newActiveState)
    {
        gameObject.SetActive(newActiveState);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
