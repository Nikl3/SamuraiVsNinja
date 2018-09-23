public class AudioPanel : UIPanel
{
    public override void BackButton()
    {
        base.BackButton();

        UIManager.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }
}
