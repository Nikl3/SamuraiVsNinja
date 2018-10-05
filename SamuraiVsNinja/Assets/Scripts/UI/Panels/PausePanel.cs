using UnityEngine;

public class PausePanel : UIPanel
{
    public override void OpenBehaviour()
    {
        Time.timeScale = 0f;
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
    }
    public override void BackButton()
    {
        base.BackButton();
        GameMaster.Instance.LoadScene(0);
    }
}
