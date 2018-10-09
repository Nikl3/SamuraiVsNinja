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
        UIManager.Instance.TriggerPanelCloseBehaviour();
        Time.timeScale = 1f;
        UIManager.Instance.PlayerInfoContainerGroup.alpha = 1f;
    }
    public override void BackButton()
    {
        base.BackButton();
        GameMaster.Instance.LoadScene(1);
    }
}
