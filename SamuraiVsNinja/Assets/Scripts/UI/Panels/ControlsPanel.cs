public class ControlsPanel : UIPanel
{
    public override void BackButton()
    {
        base.BackButton();

        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }
}
