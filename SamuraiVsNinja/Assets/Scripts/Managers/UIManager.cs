using UnityEngine;
using UnityEngine.UI;
using Fabric;

public enum PANEL_STATE
{
	MAIN_MENU,
	CHARACTER_SELECT,
	OPTIONS,
	CREDITS,
	HOW_TO_PLAY,
	AUDIO,
	GRAPHICS, 
	CONTROL, 
	PAUSE ,
	VICTORY ,
	ONLINE_LOBBY
}

public class UIManager : Singelton<UIManager>
{
	#region PROPERTIES

	public PANEL_STATE CurrentPanelState
	{
		get;
		private set;
	}
	public Transform PlayerInfoContainer { get; private set; }
	public CanvasGroup PlayerInfoContainerGroup { get; private set; }
	public Transform PlayerEndPanel { get; private set; }
	public GameObject TitleCharacters { get; private set; }
	public GameObject TitleGameObject { get; private set; }
	public GameObject PanelsGameObject { get; private set; }

	public GameObject MainMenuBackgroundImageGameObject { get; private set; }
	public Image GameTitleImage { get; private set; }
	public Image PanelBackgroundImage { get; private set; }

	public Animator Animator { get; private set; }

	public UIPanel CurrentPanel { get; private set; }
	public UIPanel MainMenuPanel { get; private set; }
	public UIPanel CharacterSelectPanel { get; private set; }
	public UIPanel OptionsPanel { get; private set; }
	public UIPanel CreditsPanel { get; private set; }
	public UIPanel HowToPlayPanel { get; private set; }
	public UIPanel AudioPanel { get; private set; }
	public UIPanel GraphicsPanel { get; private set; }
	public UIPanel ControlsPanel { get; private set; }
	public UIPanel PausePanel { get; private set; }
	public UIPanel VictoryPanel { get; private set; }
	public UIPanel OnlineLobbyPanel { get; private set; }

	#endregion PROPERTIES

	private void Awake()
	{
		Initialize();
	}
	private void Start()
	{   
		MainMenuBackgroundImageGameObject.gameObject.SetActive(true);
	}
	private void Update()
	{
		OnStartButtonDown(GameMaster.Instance.CurrentGameState);
	}

	private void Initialize()
	{
		PlayerInfoContainer = transform.Find("PlayerInfoContainer");
		PlayerInfoContainerGroup = PlayerInfoContainer.GetComponent<CanvasGroup>();
		TitleCharacters = transform.Find("TitleCharacters").gameObject;
		TitleGameObject = transform.Find("LogoImage").gameObject;
		PanelsGameObject = transform.Find("Panels").gameObject;
		MainMenuBackgroundImageGameObject = transform.Find("MenuBackgroundImage").gameObject;

		PanelBackgroundImage = PanelsGameObject.transform.GetComponent<Image>();

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

		PlayerEndPanel = VictoryPanel.transform.Find("PlayerStatsPanel").transform.Find("PlayerEndPanel");
		Animator = GetComponent<Animator>();
	}
	private void OnStartButtonDown(CURRENT_GAME_STATE currentGameState)
	{
		if (InputManager.Instance.Y_ButtonDown(1))
		{
			switch (currentGameState)
			{
				case CURRENT_GAME_STATE.MAIN_MENU:

					if (CreditsPanel.IsOpen)
					{
						TriggerPanelBehaviour(MainMenuPanel);
					}

					break;

				case CURRENT_GAME_STATE.LOCAL_GAME:

					if (!LevelManager.Instance.GameIsRunning)
						return;

					if(!PausePanel.IsOpen)
					{
						Time.timeScale = 0f;
						TriggerPanelBehaviour(PausePanel);
						PlayerInfoContainerGroup.alpha = 0f;         
					}
					else
					{
						TriggerPanelCloseBehaviour();
						Time.timeScale = 1f;
						PlayerInfoContainerGroup.alpha = 1f;
					}

					break;

				case CURRENT_GAME_STATE.ONLINE_GAME:			

					break;
			}
		}
	}
	private void TriggerPanelBehaviour(UIPanel panel)
	{
		if (CurrentPanel != null)
			CurrentPanel.CloseBehaviour();

		CurrentPanel = panel;
		CurrentPanel.OpenBehaviour();
		if (CurrentPanelState != PANEL_STATE.VICTORY)
		{
			PanelBackgroundImage.enabled = true;
		}
	}
	private void PreSetPanelsState(bool isActive)
	{
		MainMenuPanel.gameObject.SetActive(isActive);
		CharacterSelectPanel.gameObject.SetActive(isActive);
		OptionsPanel.gameObject.SetActive(isActive);
		CreditsPanel.gameObject.SetActive(isActive);
		HowToPlayPanel.gameObject.SetActive(isActive);
		AudioPanel.gameObject.SetActive(isActive);
		GraphicsPanel.gameObject.SetActive(isActive);
		ControlsPanel.gameObject.SetActive(isActive);
		PausePanel.gameObject.SetActive(isActive);
		VictoryPanel.gameObject.SetActive(isActive);
		OnlineLobbyPanel.gameObject.SetActive(isActive);
	}

	public void TriggerPanelCloseBehaviour()
	{
		if (CurrentPanel != null)
		{
			CurrentPanel.CloseBehaviour();
			PanelBackgroundImage.enabled = false;
			CurrentPanel = null;
		}		
	}
	public void ChangePanelState(PANEL_STATE newPanelState)
	{
		CurrentPanelState = newPanelState;

		switch (CurrentPanelState)
		{
			case PANEL_STATE.MAIN_MENU:
				TriggerPanelBehaviour(MainMenuPanel);
				break;

			case PANEL_STATE.CHARACTER_SELECT:
				TriggerPanelBehaviour(CharacterSelectPanel);
				break;
			case PANEL_STATE.OPTIONS:
				TriggerPanelBehaviour(OptionsPanel);
				break;

			case PANEL_STATE.CREDITS:
				TriggerPanelBehaviour(CreditsPanel);
				break;

			case PANEL_STATE.HOW_TO_PLAY:
				TriggerPanelBehaviour(HowToPlayPanel);
				PanelBackgroundImage.enabled = false;
				break;

			case PANEL_STATE.AUDIO:
				TriggerPanelBehaviour(AudioPanel);
				break;

			case PANEL_STATE.GRAPHICS:
				TriggerPanelBehaviour(GraphicsPanel);
				break;

			case PANEL_STATE.CONTROL:
				TriggerPanelBehaviour(ControlsPanel);
				PanelBackgroundImage.enabled = false;
				break;

			case PANEL_STATE.PAUSE:
				PanelBackgroundImage.enabled = true;
				TriggerPanelBehaviour(PausePanel);
				break;

			case PANEL_STATE.VICTORY:
				PlayerInfoContainerGroup.alpha = 0f;
				TriggerPanelBehaviour(VictoryPanel);
				break;

			case PANEL_STATE.ONLINE_LOBBY:
				TriggerPanelBehaviour(OnlineLobbyPanel);
				break;
		}
	}
	public void SetMainMenuUI()
	{
		TitleCharacters.SetActive(true);
		TitleGameObject.SetActive(true);
		PreSetPanelsState(false);

		MainMenuBackgroundImageGameObject.SetActive(true);
		CameraEngine.Instance.ManageLevelBackground(false);

		TriggerPanelBehaviour(MainMenuPanel);

		EventManager.Instance.PostEvent("LevelTheme", EventAction.StopSound);
		EventManager.Instance.PostEvent("MenuTheme");
	}
	public void SetLevelUI()
	{
		PreSetPanelsState(false);
		TitleCharacters.SetActive(false);
		TitleGameObject.SetActive(false);
		PlayerInfoContainer.gameObject.SetActive(true);
		PlayerInfoContainerGroup.alpha = 1f;

		MainMenuBackgroundImageGameObject.SetActive(false);
		CameraEngine.Instance.ManageLevelBackground(true);

		TriggerPanelCloseBehaviour();

		EventManager.Instance.PostEvent("MenuTheme", EventAction.StopSound);
		EventManager.Instance.PostEvent("LevelTheme");
	}
	public void SetOnlineUI()
	{
		PreSetPanelsState(false);
	}
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