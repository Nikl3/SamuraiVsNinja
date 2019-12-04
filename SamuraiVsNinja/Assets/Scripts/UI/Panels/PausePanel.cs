using UnityEngine;

public class PausePanel : UIPanel
{
    public override void OpenBehaviour()
    {
        base.OpenBehaviour();
    }
    public override void CloseBehaviour()
    {
        base.CloseBehaviour();
    }

    public void ContinueButton()
    {
        UIManager_Old.Instance.TriggerPanelCloseBehaviour();
        Time.timeScale = 1f;
        UIManager_Old.Instance.PlayerInfoContainerGroup.alpha = 1f;
    }
    public override void BackButton()
    {
        base.BackButton();
        GameManager.Instance.LoadSceneAsync(1);
    }
}
