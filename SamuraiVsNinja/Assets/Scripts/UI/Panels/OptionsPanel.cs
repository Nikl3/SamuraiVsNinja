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
        UIManager.Instance.ChangePanelState(PANEL_STATE.HOW_TO_PLAY);
    }

    public void AudioButton()
    {
        UIManager.Instance.ChangePanelState(PANEL_STATE.AUDIO);
    }

    public void GraphicsButton()
    {
        UIManager.Instance.ChangePanelState(PANEL_STATE.GRAPHICS);
    }

    public void ControlsButton()
    {
        UIManager.Instance.ChangePanelState(PANEL_STATE.CONTROL);
    }

    public override void BackButton()
    {
        base.BackButton();

        UIManager.Instance.ChangePanelState(GameMaster.Instance.CurrentGameState == CURRENT_GAME_STATE.MAIN_MENU ? PANEL_STATE.MAIN_MENU : PANEL_STATE.PAUSE);
    }
}
