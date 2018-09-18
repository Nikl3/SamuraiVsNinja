using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

	private PanelState currentPanelState;

	private readonly Coroutine[] coroutines = new Coroutine[4];

	private GameObject menuCanvasGameObject;
	private GameObject panels;
	private GameObject characterSelectContainer;


	private UIPanel mainMenuPanel;
	private UIPanel optionsPanel;
	private UIPanel creditsPanel;
	private UIPanel howToPlayPanel;
	private UIPanel audioPanel;
	private UIPanel graphicsPanel;
	private UIPanel controlsPanel;
	private UIPanel characterSelectPanel;

	private JoinField[] joinFields;

	private Text joinedPlayerText;

	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";
	private bool isCreditsRunning = false;

	#endregion VARIABLES

	#region PROPERTIES

	public string CreditsAnimationTag
	{
		get
		{
			return creditsAnimationTag;
		}

		set
		{
			creditsAnimationTag = value;
		}
	}
	public bool CanJoin
	{
		get;
		private set;
	}

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
		menuCanvasGameObject = GameObject.Find("MainMenuCanvas");

		panels = menuCanvasGameObject.transform.Find("Panels").gameObject;
		mainMenuPanel = panels.transform.Find("MainMenuPanel").GetComponent< UIPanel>();
		optionsPanel = panels.transform.Find("OptionsPanel").GetComponent<UIPanel>();
		creditsPanel = panels.transform.Find("CreditsPanel").GetComponent<UIPanel>();
		howToPlayPanel = panels.transform.Find("HowToPlayPanel").GetComponent<UIPanel>();
		audioPanel = panels.transform.Find("AudioPanel").GetComponent<UIPanel>();
		graphicsPanel = panels.transform.Find("GraphicsPanel").GetComponent<UIPanel>();
		controlsPanel = panels.transform.Find("ControlsPanel").GetComponent<UIPanel>();
		characterSelectPanel = panels.transform.Find("CharacterSelectPanel").GetComponent<UIPanel>();
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
		InputManager.Instance.FocusMenuPanel();

		if (isCreditsRunning)
		{
			CancelCredits();
			return;
		}

		if (CanJoin)
		{
			HadlePlayerJoinings();
			HandleCharacterChange();
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
				InputManager.Instance.ChangeActiveSelectedObject(mainMenuPanel.panelDefaultButton.gameObject);

				break;

			case PanelState.CHARACTER_SELECT:

				mainMenuPanel.SetActive(false);

				characterSelectPanel.SetActive(true);
				CanJoin = true;
				optionsPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(characterSelectPanel.panelDefaultButton.gameObject);

				break;

			case PanelState.OPTIONS:

				mainMenuPanel.SetActive(false);
				howToPlayPanel.SetActive(false);
				audioPanel.SetActive(false);
				graphicsPanel.SetActive(false);
				controlsPanel.SetActive(false);

				optionsPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(optionsPanel.panelDefaultButton.gameObject);

				break;

			case PanelState.CREDITS:

				isCreditsRunning = true;
				mainMenuCanvasAnimator.Play("Credits");

				break;

			case PanelState.HOW_TO_PLAY:

				optionsPanel.SetActive(false);

				howToPlayPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(howToPlayPanel.panelDefaultButton.gameObject);

				break;

			case PanelState.AUDIO:

				optionsPanel.SetActive(false);

				audioPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(audioPanel.panelDefaultButton.gameObject);

				break;

			case PanelState.GRAPHICS:

				optionsPanel.SetActive(false);

				graphicsPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(graphicsPanel.panelDefaultButton.gameObject);


				break;

			case PanelState.CONTROLS:

				controlsPanel.SetActive(true);
				InputManager.Instance.ChangeActiveSelectedObject(controlsPanel.panelDefaultButton.gameObject);

				optionsPanel.SetActive(false);

				break;

			default:

				ChangePanelState(PanelState.MAIN_MENU);

				break;
		}
	}

	private void CancelCredits()
	{		
		if (InputManager.Instance.Y_ButtonDown(1) && isCreditsRunning)
		{
			if (mainMenuCanvasAnimator.GetCurrentAnimatorStateInfo(0).IsTag(CreditsAnimationTag))
			{
				mainMenuCanvasAnimator.Play("Credits", 0, 0.98f);
				isCreditsRunning = false;
			}

			ChangePanelState(PanelState.MAIN_MENU);
		}
	}

	private void HadlePlayerJoinings()
	{
		for (int i = 0; i < joinFields.Length; i++)
		{
			if (!joinFields[i].HasJoined)
			{
				if (InputManager.Instance.Start_ButtonDown(i + 1))
				{
					coroutines[i] = StartCoroutine(IChangeCharacter(i + 1));
					PlayerDataManager.Instance.PlayerJoin(i + 1);
					SetJoinField(i + 1, PlayerDataManager.Instance.GetPlayerData(i).PlayerColor);
				}
			}
		}

		for (int i = 0; i < joinFields.Length; i++)
		{
			if (joinFields[i].HasJoined)
			{
				if (InputManager.Instance.Y_ButtonDown(i + 1))
				{
					StopCoroutine(coroutines[i]);
					PlayerDataManager.Instance.PlayerUnjoin(i + 1);
					UnSetJoinField(i + 1);
				}
			}
		}
	}

	private void HandleCharacterChange()
	{
		for (int i = 0; i < joinFields.Length; i++)
		{
			if (joinFields[i].HasJoined)
				InputManager.Instance.GetHorizontalAxisRaw(i + 1);
		}
	}

	private void ChangePlayerIcon(JoinField joinField, PlayerData playerData)
	{
		

	}

	public void SetJoinField(int playerID, Color joinColor)
	{		
		joinFields[playerID - 1].ChangeJoinFieldVisuals(playerID, joinColor);
		joinedPlayerText.text = "PLAYERS " + PlayerDataManager.Instance.CurrentlyJoinedPlayers + " / " + 4;
	}

	public void UnSetJoinField(int playerID)
	{		
		joinFields[playerID - 1].UnChangeJoinFieldVisuals();         
		joinedPlayerText.text = "PLAYERS " + PlayerDataManager.Instance.CurrentlyJoinedPlayers + " / " + 4;
	}

	public void UnSetAllJoinField()
	{
		foreach (var coroutine in coroutines)
		{
			if(coroutine != null)
			StopCoroutine(coroutine);
		}

		for (int i = 0; i < joinFields.Length; i++)
		{
			joinFields[i].UnChangeJoinFieldVisuals();
		}

		joinedPlayerText.text = "PLAYERS 0 / " + PlayerDataManager.Instance.MaxPlayerNumber;
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
		CanJoin = false;
		InputManager.Instance.ChangeActiveSelectedObject(null);
		SceneMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		InputManager.Instance.ChangeActiveSelectedObject(null);
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
		ChangePanelState(PanelState.CREDITS);
	}

	public void BackToOptionsButton()
	{
		ChangePanelState(PanelState.OPTIONS);
	}

	public void BackToMenuButton()
	{
		if (currentPanelState == PanelState.CHARACTER_SELECT)
		{
			CanJoin = false;
			PlayerDataManager.Instance.ClearPlayerDataIndex();
			UnSetAllJoinField();      
		}

		ChangePanelState(PanelState.MAIN_MENU);
	}

	public void QuitButton()
	{
		SceneMaster.Instance.ExitGame(() => OnQuit());
	}

	#endregion UI BUTTONS

	private IEnumerator IChangeCharacter(int id)
	{
		while (true)
		{
			yield return new WaitUntil(() => InputManager.Instance.GetHorizontalAxisRaw(id) == 0);
			ChangePlayerIcon(joinFields[id - 1], (PlayerDataManager.Instance.GetPlayerData(id - 1)));
			yield return new WaitUntil(() => InputManager.Instance.GetHorizontalAxisRaw(id) != 0);
		}		
	}
}
