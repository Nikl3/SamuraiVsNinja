public class OptionsPanel : UIPanel
{
    public override void OpenBehaviour()
    {
        base.OpenBehaviour();
    }

    public override void CloseBehaviour()
    {
        base.CloseBehaviour();
    }

    public void HowToPlayButton()
    {
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.HOW_TO_PLAY);
    }

    public void AudioButton()
    {
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.AUDIO);
    }

    public void GraphicsButton()
    {
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.GRAPHICS);
    }

    public void ControlsButton()
    {
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.CONTROL);
    }

    public override void BackButton()
    {
        base.BackButton();

        UIManager_Old.Instance.ChangePanelState(GameManager.Instance.CurrentGameState == GAME_STATE_OLD.MAIN_MENU ? PANEL_STATE.MAIN_MENU : PANEL_STATE.PAUSE);
    }
}
