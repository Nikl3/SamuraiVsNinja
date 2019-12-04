using UnityEditor;

public class MainMenuPanel : UIPanel
{
    public override void Update()
    {
        if (InputManager.Instance.Start_ButtonDown(1))
        {
            GameManager.Instance.LoadScene(0);                         
        }

        base.Update();
    }

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
		UIManager_Old.Instance.ChangePanelState(PANEL_STATE.CHARACTER_SELECT);
	}

	public void OnlineButton()
	{
		GameManager.Instance.LoadSceneAsync(3);
	}

	public void CreditsButton()
	{
		UIManager_Old.Instance.ChangePanelState(PANEL_STATE.CREDITS);
	}

	public override void BackButton()
	{
		base.BackButton();
		GameManager.Instance.ExitGame(() => OnQuit());
	}
}
