using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
	#region VARIABLES

	public Sprite[] BackgroundSprites;

	#endregion VARIABLES

	#region PROPERTIES

	[SerializeField] public UIPanel CurrentPanel { get; private set; }
	[SerializeField] public GameObject TitleCharacters { get; private set; }
	[SerializeField] public GameObject TitleGameObject { get; private set; }
	[SerializeField] public GameObject PanelsGameObject { get; private set; }
	[SerializeField] public Image BackgroundImage { get; private set; }

	[SerializeField] public UIPanel MainMenuPanel { get; private set; }
	[SerializeField] public UIPanel CharacterSelectPanel { get; private set; }
	[SerializeField] public UIPanel OptionsPanel { get; private set; }
	[SerializeField] public UIPanel CreditsPanel { get; private set; }
	[SerializeField] public UIPanel HowToPlayPanel { get; private set; }
	[SerializeField] public UIPanel AudioPanel { get; private set; }
	[SerializeField] public UIPanel GraphicsPanel { get; private set; }
	[SerializeField] public UIPanel ControlsPanel { get; private set; }
	[SerializeField] public UIPanel PausePanel { get; private set; }
	[SerializeField] public UIPanel VictoryPanel { get; private set; }
	[SerializeField] public UIPanel OnlineLobbyPanel { get; private set; }

	public Animator UIManagerAnimator { get; private set; }

	#endregion PROPERTIES

	private void Awake()
	{
		TitleCharacters = transform.Find("TitleCharacters").gameObject;
		TitleGameObject = transform.Find("Title").gameObject;
		PanelsGameObject = transform.Find("Panels").gameObject;
		BackgroundImage = transform.Find("BackgroundImage").GetComponent<Image>();

		PanelsGameObject = transform.Find("Panels").gameObject;
		MainMenuPanel = PanelsGameObject.transform.Find("MainMenuPanel").GetComponent<UIPanel>();
		CharacterSelectPanel = PanelsGameObject.transform.Find("CharacterSelectPanel").GetComponent<UIPanel>();
		OptionsPanel = PanelsGameObject.transform.Find("OptionsPanel").GetComponent<UIPanel>();
		CreditsPanel = PanelsGameObject.transform.Find("CreditsPanel").GetComponent<UIPanel>();
		HowToPlayPanel = PanelsGameObject.transform.Find("HowToPlayPanel").GetComponent<UIPanel>();
		AudioPanel = PanelsGameObject.transform.Find("AudioPanel").GetComponent<UIPanel>();
		GraphicsPanel = PanelsGameObject.transform.Find("GraphicsPanel").GetComponent<UIPanel>();
		ControlsPanel = PanelsGameObject.transform.Find("ControlsPanel").GetComponent<UIPanel>();
		PausePanel = PanelsGameObject.transform.Find("PausePanel").GetComponent<UIPanel>();
		VictoryPanel = PanelsGameObject.transform.Find("VictoryPanel").GetComponent<UIPanel>();
		OnlineLobbyPanel = PanelsGameObject.transform.Find("OnlineLobbyPanel").GetComponent<UIPanel>();

		UIManagerAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		if(CurrentPanel != null)
		InputManager.Instance.FocusMenuPanel();

		if (InputManager.Instance.Start_ButtonDown(1))
		{
			if (GameMaster.Instance.CurrentSceneName != "MainMenu")
				TriggerPanelBehaviour(PausePanel);
		}
	}

	private void TriggerPanelBehaviour(UIPanel panel)
	{
		if (CurrentPanel != null)
			CurrentPanel.CloseBehaviour();

		CurrentPanel = panel;
		CurrentPanel.OpenBehaviour();
	}

	public void SetMainMenuUI()
	{
		TriggerPanelBehaviour(MainMenuPanel);
		BackgroundImage.sprite = BackgroundSprites[0];
		TitleCharacters.SetActive(true);
		TitleGameObject.SetActive(true);
	}

	public void SetLevelUI()
	{
		if(CurrentPanel != null)
			CurrentPanel.CloseBehaviour();

		BackgroundImage.sprite = BackgroundSprites[1];
		TitleCharacters.SetActive(false);
		TitleGameObject.SetActive(false);
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
		TriggerPanelBehaviour(CharacterSelectPanel);
	}

	public void StartButton()
	{
		GameMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		GameMaster.Instance.LoadScene(2);
	}

	public void OptionsButton()
	{
		TriggerPanelBehaviour(OptionsPanel);
	}

	public void HowToPlayButton()
	{
		TriggerPanelBehaviour(HowToPlayPanel);
	}

	public void AudioButton()
	{
		TriggerPanelBehaviour(AudioPanel);
	}

	public void GraphicsButton()
	{
		TriggerPanelBehaviour(GraphicsPanel);
	}

	public void ControlsButton()
	{
		TriggerPanelBehaviour(ControlsPanel);
	}

	public void CreditsButton()
	{
		TriggerPanelBehaviour(CreditsPanel);
	}

	public void BackToOptionsButton()
	{
		TriggerPanelBehaviour(OptionsPanel);
	}

	public void BackToMainMenuButton()
	{
		TriggerPanelBehaviour(MainMenuPanel);
	}

	public void QuitButton()
	{
		GameMaster.Instance.ExitGame(() => OnQuit());
	}

	#endregion MAINMENU_BUTTONS

	#region INGAME_SCENE_BUTTONS

	public void ContinueButton()
	{
		if(CurrentPanel != null)
		{
			CurrentPanel.CloseBehaviour();
		}
	}

	public void RestartGameButton()
	{
		GameMaster.Instance.LoadScene(1);
	}

	public void BackToMainMenuSceneButton()
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