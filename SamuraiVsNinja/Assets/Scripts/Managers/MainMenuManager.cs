using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PanelState
{
	MAIN_MENU,
	CHARACTER_SELECT,
	OPTIONS,
	CREDITS,
	HOW_TO_PLAY,
	AUDIO,
	GRAPHICS,
	CONTROLS
}

public class MainMenuManager : Singelton<MainMenuManager>
{
	#region VARIABLES

	private bool inSubPanel = false;

	private PanelState currentPanelState;

	private EventSystem eventSystem;

	private GameObject menuCanvasGameObject;
	private GameObject panels;
	private GameObject mainMenuPanel;
	private GameObject optionsPanel;
	private GameObject creditsPanel;
	private GameObject howToPlayPanel;
	private GameObject audioPanel;
	private GameObject graphicsPanel;
	private GameObject controlsPanel;
	private GameObject characterSelectPanel;

	private GameObject characterSelectContainer;

	private JoinField[] joinFields;
	private Color fieldColor = Color.green;

	[SerializeField]
	private GameObject[] panelDefaultButtonObjcts;
	private GameObject previousButtonObject;
	private Text joinedPlayerText;

	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";
	private bool isCreditsRunning = false;

	#endregion VARIABLES

	#region PROPERTIES

	#endregion PROPERTIES

	//private bool CanStart()
	//{
	//	return startButton.interactable = PlayerDataManager.Instance.CurrentlyJoinedPlayers == 4 || PlayerDataManager.Instance.CurrentlyJoinedPlayers == 2 ? true : false;
	//}

	private JoinField[] GetJoinFields()
	{
		return characterSelectContainer.GetComponentsInChildren<JoinField>(true);
	}

	private void Initialize()
	{
		eventSystem = EventSystem.current;

		menuCanvasGameObject = GameObject.Find("MainMenuCanvas");

		panels = menuCanvasGameObject.transform.Find("Panels").gameObject;
		mainMenuPanel = panels.transform.Find("MainMenuPanel").gameObject;
		optionsPanel = panels.transform.Find("OptionsPanel").gameObject;
		creditsPanel = panels.transform.Find("CreditsPanel").gameObject;
		howToPlayPanel = panels.transform.Find("HowToPlayPanel").gameObject;
		audioPanel = panels.transform.Find("AudioPanel").gameObject;
		graphicsPanel = panels.transform.Find("GraphicsPanel").gameObject;
		controlsPanel = panels.transform.Find("ControlsPanel").gameObject;
		characterSelectPanel = panels.transform.Find("CharacterSelectPanel").gameObject;
		characterSelectContainer = characterSelectPanel.transform.Find("CharacterSelectContainer").gameObject;

		joinFields = GetJoinFields();

		joinedPlayerText = characterSelectPanel.transform.Find("JoinedPlayers").GetComponent<Text>();
		//startButton = characterSelectContainer.transform.Find("StartButton").GetComponent<Button>();
		mainMenuCanvasAnimator = menuCanvasGameObject.GetComponent<Animator>();
	}

	private void Awake()
	{
		Initialize();
	}

	private void Start()
	{
		ChangePanelState(PanelState.MAIN_MENU);
	}

	private void Update()
	{
		//if (eventSystem.currentSelectedGameObject == null)
		//{
		//	if(Input.anyKeyDown)
		//	{
		//		SetDefaultButtonObject(0);
		//	}
		//}

		if (isCreditsRunning)
		{
			CancelCredits();
			return;
		}

		if (PlayerDataManager.Instance.CanJoin)
		{
			HadlePlayerJoinings();
		}
	}

	private void ChangePanelState(PanelState newPanelState)
	{
		currentPanelState = newPanelState;

		switch (currentPanelState)
		{
			case PanelState.MAIN_MENU:

				characterSelectPanel.SetActive(false);
				optionsPanel.SetActive(false);
				creditsPanel.SetActive(false);
				howToPlayPanel.SetActive(false);
				audioPanel.SetActive(false);
				graphicsPanel.SetActive(false);
				controlsPanel.SetActive(false);

				mainMenuPanel.SetActive(true);

				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[0]);

				break;

			case PanelState.CHARACTER_SELECT:

				mainMenuPanel.SetActive(false);
				characterSelectPanel.SetActive(true);

				PlayerDataManager.Instance.CanJoin = true;

				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[1]);

				break;

			case PanelState.OPTIONS:

				optionsPanel.SetActive(true);
				mainMenuPanel.SetActive(false);
				howToPlayPanel.SetActive(false);
				audioPanel.SetActive(false);
				graphicsPanel.SetActive(false);
				controlsPanel.SetActive(false);

				eventSystem.SetSelectedGameObject(inSubPanel ? previousButtonObject : panelDefaultButtonObjcts[2]);

				inSubPanel = false;


				break;

			case PanelState.CREDITS:

				//creditsPanel.SetActive(true);

				//mainMenuPanel.SetActive(false);

				//isCreditsRunning = true;
				//mainMenuCanvasAnimator.Play("Credits");

				break;

			case PanelState.HOW_TO_PLAY:

				howToPlayPanel.SetActive(true);
				optionsPanel.SetActive(false);

				previousButtonObject = eventSystem.currentSelectedGameObject;	
				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[3]);
				inSubPanel = true;

				break;

			case PanelState.AUDIO:

				audioPanel.SetActive(true);
				optionsPanel.SetActive(false);

				previousButtonObject = eventSystem.currentSelectedGameObject;
				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[4]);

				inSubPanel = true;

				break;

			case PanelState.GRAPHICS:

				graphicsPanel.SetActive(true);
				optionsPanel.SetActive(false);

				previousButtonObject = eventSystem.currentSelectedGameObject;
				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[5]);

				inSubPanel = true;

				break;

			case PanelState.CONTROLS:

				controlsPanel.SetActive(true);
				optionsPanel.SetActive(false);

				previousButtonObject = eventSystem.currentSelectedGameObject;
				eventSystem.SetSelectedGameObject(panelDefaultButtonObjcts[6]);

				inSubPanel = true;

				break;

			default:

				ChangePanelState(PanelState.MAIN_MENU);

				break;
		}
	}

	private void CancelCredits()
	{		
		if (Input.GetButtonUp("Cancel_J1") && isCreditsRunning)
		{
			if (mainMenuCanvasAnimator.GetCurrentAnimatorStateInfo(0).IsTag(creditsAnimationTag))
			{
				mainMenuCanvasAnimator.Play("Credits", 0, 0.98f);
				isCreditsRunning = false;
			}

			ChangePanelState(PanelState.MAIN_MENU);
		}
	}

	public void HadlePlayerJoinings()
	{
		if (Input.GetButtonDown("Action_J1"))
		{
			PlayerDataManager.Instance.PlayerJoin(0);
			SetJoinField(1);
		}

		if (Input.GetButtonDown("Action_J2"))
		{
			PlayerDataManager.Instance.PlayerJoin(1);
			SetJoinField(2);
		}

		if (Input.GetButtonDown("Action_J3"))
		{
			PlayerDataManager.Instance.PlayerJoin(2);
			SetJoinField(3);
		}

		if (Input.GetButtonDown("Action_J4"))
		{
			PlayerDataManager.Instance.PlayerJoin(3);
			SetJoinField(4);
		}

		if (Input.GetButtonDown("Cancel_J1"))
		{
			PlayerDataManager.Instance.PlayerUnjoin(0);
			UnSetJoinField(1);
		}

		if (Input.GetButtonDown("Cancel_J2"))
		{
			PlayerDataManager.Instance.PlayerUnjoin(1);
			UnSetJoinField(2);
		}

		if (Input.GetButtonDown("Cancel_J3"))
		{
			PlayerDataManager.Instance.PlayerUnjoin(2);
			UnSetJoinField(3);
		}

		if (Input.GetButtonDown("Cancel_J4"))
		{
			PlayerDataManager.Instance.PlayerUnjoin(3);
			UnSetJoinField(4);
		}
	}

	public void SetJoinField(int playerID)
	{
		joinFields[playerID - 1].ChangeJoinFieldVisuals(playerID, fieldColor);
		joinedPlayerText.text = "PLAYERS " + (playerID) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
		//CanStart();
	}

	public void UnSetJoinField(int playerID)
	{
		for (int i = 0; i < joinFields.Length; i++)
		{
			joinFields[i].UnChangeJoinFieldVisuals();         
		}

		joinedPlayerText.text = "PLAYERS " + (PlayerDataManager.Instance.CurrentlyJoinedPlayers) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
		//CanStart();
	}

	public void UnSetAllJoinField(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			joinFields[i].UnChangeJoinFieldVisuals();
		}

		joinedPlayerText.text = "PLAYERS 0 / " + PlayerDataManager.Instance.MaxPlayerNumber;
		//CanStart();
	}

	private void OnQuit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#region UI BUTTONS

	public void PlayButton()
	{
		ChangePanelState(PanelState.CHARACTER_SELECT);
	}

	public void StartButton()
	{
		PlayerDataManager.Instance.CanJoin = false;
		SceneMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		SceneMaster.Instance.LoadScene(2);
	}

	public void OptionsButton()
	{
		ChangePanelState(PanelState.OPTIONS);
	}

	public void HowToPlayButton()
	{
		ChangePanelState(PanelState.HOW_TO_PLAY);
	}

	public void AudioButton()
	{
		ChangePanelState(PanelState.AUDIO);
	}

	public void GraphicsButton()
	{
		ChangePanelState(PanelState.GRAPHICS);
	}

	public void ControlsButton()
	{
		ChangePanelState(PanelState.CONTROLS);
	}

	public void CreditsButton()
	{
		// ChangePanelState(PanelState.CREDITS);
	}

	public void BackToOptionsButton()
	{
		ChangePanelState(PanelState.OPTIONS);
	}

	public void BackToMenuButton()
	{
		if (characterSelectPanel.activeSelf)
		{
			PlayerDataManager.Instance.CanJoin = false;
			PlayerDataManager.Instance.ClearPlayerDataIndex();
			UnSetAllJoinField(joinFields.Length);
		}

		ChangePanelState(PanelState.MAIN_MENU);
	}

	public void QuitButton()
	{
		SceneMaster.Instance.ExitGame(() => OnQuit());
	}

	#endregion UI BUTTONS
}
