using UnityEngine;
using UnityEngine.UI;

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
	#region VARIABLES

	public Sprite[] BackgroundSprites;

	#endregion VARIABLES

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
	public GameObject Fireflies { get; private set; }
	public Image GameTitleImage { get; private set; }
	public Image PanelBackgroundImage { get; private set; }
	public Image BackgroundImage { get; private set; }
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
		BackgroundImage.gameObject.SetActive(true);
	}
	private void Update()
	{
		OnStartButtonDown(GameMaster.Instance.CurrentGameState);
	}

	private void Initialize()
	{
		Fireflies = transform.Find("Fireflies").gameObject;
		PlayerInfoContainer = transform.Find("PlayerInfoContainer");
		PlayerInfoContainerGroup = PlayerInfoContainer.GetComponent<CanvasGroup>();
		TitleCharacters = transform.Find("TitleCharacters").gameObject;
		TitleGameObject = transform.Find("LogoImage").gameObject;
		PanelsGameObject = transform.Find("Panels").gameObject;
		PanelBackgroundImage = PanelsGameObject.transform.GetComponent<Image>();
		BackgroundImage = transform.Find("BackgroundImage").GetComponent<Image>();

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

					if(!PausePanel.IsOpen)
					{
						TriggerPanelBehaviour(PausePanel);
					}
					else
					{
						TriggerPanelCloseBehaviour();
						Time.timeScale = 1f;
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

		PanelBackgroundImage.enabled = true;
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
				break;

			case PANEL_STATE.AUDIO:
				TriggerPanelBehaviour(AudioPanel);
				break;

			case PANEL_STATE.GRAPHICS:
				TriggerPanelBehaviour(GraphicsPanel);
				break;

			case PANEL_STATE.CONTROL:
				TriggerPanelBehaviour(ControlsPanel);
				break;

			case PANEL_STATE.PAUSE:
				TriggerPanelBehaviour(PausePanel);
				break;

			case PANEL_STATE.VICTORY:
				PlayerInfoContainer.gameObject.SetActive(false);
				TriggerPanelBehaviour(VictoryPanel);
				break;

			case PANEL_STATE.ONLINE_LOBBY:
				TriggerPanelBehaviour(OnlineLobbyPanel);
				break;
		}
	}
	public void SetMainMenuUI()
	{
		Fireflies.SetActive(true);
		TitleCharacters.SetActive(true);
		TitleGameObject.SetActive(true);
		PreSetPanelsState(false);
		BackgroundImage.sprite = BackgroundSprites[0];

		TriggerPanelBehaviour(MainMenuPanel);
	}
	public void SetLevelUI()
	{
		Fireflies.SetActive(false);
		PreSetPanelsState(false);
		TitleCharacters.SetActive(false);
		TitleGameObject.SetActive(false);
		PlayerInfoContainer.gameObject.SetActive(true);

		BackgroundImage.sprite = BackgroundSprites[1];
		TriggerPanelCloseBehaviour();
	}
	public void SetOnlineUI()
	{
		Fireflies.SetActive(false);
		PreSetPanelsState(false);
	}
	public void ClearPlayerInfoContainer()
	{
		foreach (Transform playerInfo in PlayerInfoContainer)
		{
			if(playerInfo != null)
			{
				Destroy(playerInfo.gameObject);
			}
		}

		foreach (Transform endStats in PlayerEndPanel)
		{
			if (endStats != null)
			{
				Destroy(endStats.gameObject);
			}
		}
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