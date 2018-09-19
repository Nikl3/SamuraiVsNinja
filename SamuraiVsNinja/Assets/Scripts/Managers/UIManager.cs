using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
	public UIPanel CurrentPanel;

	public Image[] bgImages;


	#region VARIABLES

	[SerializeField] private UIPanel mainMenuPanel;
	[SerializeField] private UIPanel characterSelectPanel;
	[SerializeField] private UIPanel optionsPanel;
	[SerializeField] private UIPanel creditsPanel;
	[SerializeField] private UIPanel howToPlayPanel;
	[SerializeField] private UIPanel audioPanel;
	[SerializeField] private UIPanel graphicsPanel;
	[SerializeField] private UIPanel controlsPanel;
	[SerializeField] private UIPanel pausePanel;
	[SerializeField] private UIPanel victoryPanel;
	[SerializeField] private UIPanel onlineLobbyPanel;

	//public Animator MainMenuCanvasAnimator;


	#endregion VARIABLES

	private void Start()
	{
		//TriggerOpenBehaviour(mainMenuPanel);
	}

	private void Update()
	{
		InputManager.Instance.FocusMenuPanel();
	}

	private void TriggerOpenBehaviour(UIPanel panel)
	{
		if (CurrentPanel != null)
			CurrentPanel.CloseBehaviour();

		CurrentPanel = panel;
		CurrentPanel.OpenBehaviour();
	}

	public void TriggerMainMenuBehaviour()
	{
		if (CurrentPanel != null)
			CurrentPanel.CloseBehaviour();

		CurrentPanel = mainMenuPanel;
		CurrentPanel.OpenBehaviour();
	}

	private void OnQuit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#region UI_PANEL_BUTTONS

	#region MAINMENU_BUTTONS

	public void PlayButton()
	{
		TriggerOpenBehaviour(characterSelectPanel);
	}

	public void StartButton()
	{
		InputManager.Instance.ChangeActiveSelectedObject(null);
		GameMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		InputManager.Instance.ChangeActiveSelectedObject(null);
		GameMaster.Instance.LoadScene(2);
	}

	public void OptionsButton()
	{
		TriggerOpenBehaviour(optionsPanel);
	}

	public void HowToPlayButton()
	{
		TriggerOpenBehaviour(howToPlayPanel);
	}

	public void AudioButton()
	{
		TriggerOpenBehaviour(audioPanel);
	}

	public void GraphicsButton()
	{
		TriggerOpenBehaviour(graphicsPanel);
	}

	public void ControlsButton()
	{
		TriggerOpenBehaviour(controlsPanel);
	}

	public void CreditsButton()
	{
		TriggerOpenBehaviour(creditsPanel);
	}

	public void BackToOptionsButton()
	{
		TriggerOpenBehaviour(optionsPanel);
	}

	public void BackToMenuButton()
	{
		TriggerOpenBehaviour(mainMenuPanel);
	}

	public void QuitButton()
	{
		GameMaster.Instance.ExitGame(() => OnQuit());
	}

	#endregion MAINMENU_BUTTONS

	#region INGAME_SCENE_BUTTONS

	public void ContinueButton()
	{
		//pausePanel.SetActive(false);
		//isPaused = false;
		//Time.timeScale = 1;
	}

	public void RestartGameButton()
	{
		GameMaster.Instance.LoadScene(1);
	}

	public void BackToMainMenuButton()
	{
		GameMaster.Instance.LoadScene(0);
	}

	#endregion INGAME_SCENE_BUTTONS

	#region ONLINE_SCENE_BUTTONS

	#endregion ONLINE_SCENE_BUTTONS

	#endregion UI_PANEL_BUTTONS	

	/// <summary>
	/// Temp stuff
	/// </summary>

	/*
	private GameObject pausePanel;
	private GameObject victoryPanel;
	private Text wienerText;
	private bool isPaused;

	private void Awake()
	{
	pausePanel = transform.Find("PausePanel").gameObject;
	victoryPanel = transform.Find("VictoryPanel").gameObject;
	wienerText = victoryPanel.transform.Find("WinnerText").GetComponent<Text>();
	}

	private void Start()
	{
	pausePanel.SetActive(false);
	victoryPanel.SetActive(false);
	}

	private void Update()
	{
	if (InputManager.Instance.Start_ButtonDown(1))
	{
		if (isPaused)
		{
			ContinueButton();
		}
		else
		{
			Paused();
		}
	}
	}

	public void VictoryPanel(string winnerName)
	{
	victoryPanel.SetActive(true);
	wienerText.text = winnerName + "\nYou are THE winner!";
	Time.timeScale = 0;
	}

	private void Paused()
	{
	pausePanel.SetActive(true);
	isPaused = true;
	Time.timeScale = 0;
	}
	}



	[SerializeField]
	private Transform HUDCanvas;

	[SerializeField]
	private GameObject lobbyPanel;
	public GameObject LobbyPanel
	{
		get
		{
			return lobbyPanel;
		}
		set
		{
			lobbyPanel = value;
		}
	}

	[SerializeField]
	private readonly Transform matchContainer;
	public Transform MatchContainer
	{
		get
		{
			return matchContainer;
		}
	}

	[SerializeField]
	private readonly GameObject matchButtonPrefab;
	public GameObject MatchButtonPrefab
	{
		get
		{
			return matchButtonPrefab;
		}
	}

	[SerializeField]
	private Button leaveMatchButton;
	public Button LeaveMatchButtonObject
	{
		get
		{
			return leaveMatchButton;
		}
		set
		{
			leaveMatchButton = value;
		}
	}

	[SerializeField]
	private Text statusText;
	public string StatusText
	{
		get
		{
			return statusText.text;
		}
		set
		{
			statusText.text = value;
		}
	}

	[SerializeField]
	private Text debugText;
	public string DebugText
	{
		get
		{
			return debugText.text;
		}
		set
		{
			debugText.text = value;
		}
	}

	[SerializeField]
	private Text timerText;
	public Text TimerText
	{
		get
		{
			return timerText;
		}
		set
		{
			timerText = value;
		}
	}

	public Color TimerTextColor
	{
		get
		{
			return TimerText.color;
		}
	}

	private void Start()
	{
		HUDCanvas.gameObject.SetActive(true);
		LobbyPanel.SetActive(true);
		LeaveMatchButtonObject.gameObject.SetActive(false);
	}

	public void ModifyTimerText(string message, Color newColor)
	{
		TimerText.text = message;
		TimerText.color = newColor;
	}

	#region UI_BUTTONS

	public void ChangeMachNameButton(string newMatchName)
	{
		MainNetworkManager.Instance.MatchName = newMatchName;
	}

	public void ChangeMachSizeButton(int newMatchSize)
	{
		newMatchSize = newMatchSize == 0 ? 2 : 4;
		MainNetworkManager.Instance.MatchSize = (uint)newMatchSize;
	}

	public void CreateMatchButton()
	{
		MainNetworkManager.Instance.CreateMatch();
	}

	public void RefreshMatchListButton()
	{
		MainNetworkManager.Instance.RefreshMatches();
	}

	public void LeaveMatchButton()
	{
		MainNetworkManager.Instance.LeaveMatch();
		LobbyPanel.SetActive(true);
		LeaveMatchButtonObject.gameObject.SetActive(false);
	}

	public void BackToMenuButton()
	{
		SceneMaster.Instance.LoadScene(0);
	}

	#endregion UI_BUTTONS
	}
	*/
}