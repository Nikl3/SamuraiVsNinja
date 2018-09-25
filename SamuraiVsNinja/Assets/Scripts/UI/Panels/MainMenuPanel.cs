using UnityEditor;

public class MainMenuPanel : UIPanel
{
	private void OnQuit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void PlayButton()
	{
		UIManager.Instance.ChangePanelState(PANEL_STATE.CHARACTER_SELECT);
	}

	public void OnlineButton()
	{
		GameMaster.Instance.LoadScene(2);
	}

	public void CreditsButton()
	{
		UIManager.Instance.ChangePanelState(PANEL_STATE.CREDITS);
	}

	public override void BackButton()
	{
		base.BackButton();
		GameMaster.Instance.ExitGame(() => OnQuit());
	}
}
