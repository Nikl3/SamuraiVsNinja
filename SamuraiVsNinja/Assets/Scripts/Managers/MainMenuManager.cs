using System.Collections;
using System.Collections.Generic;
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

				break;

			case PanelState.CHARACTER_SELECT:

				mainMenuPanel.SetActive(false);
				characterSelectPanel.SetActive(true);

				CanJoin = true;

				break;

			case PanelState.OPTIONS:

				optionsPanel.SetActive(true);
				mainMenuPanel.SetActive(false);
				howToPlayPanel.SetActive(false);
				audioPanel.SetActive(false);
				graphicsPanel.SetActive(false);
				controlsPanel.SetActive(false);

				break;

			case PanelState.CREDITS:

				isCreditsRunning = true;
				mainMenuCanvasAnimator.Play("Credits");

				break;

			case PanelState.HOW_TO_PLAY:

				howToPlayPanel.SetActive(true);
				optionsPanel.SetActive(false);

				break;

			case PanelState.AUDIO:

				audioPanel.SetActive(true);
				optionsPanel.SetActive(false);

				break;

			case PanelState.GRAPHICS:

				graphicsPanel.SetActive(true);
				optionsPanel.SetActive(false);

				break;

			case PanelState.CONTROLS:

				controlsPanel.SetActive(true);
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

		//InputManager.Instance.ChangeActiveSelectedObject(2);
	}

	public void StartButton()
	{
		CanJoin = false;
		InputManager.Instance.CurrentSeletedObject = null;
		SceneMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		InputManager.Instance.CurrentSeletedObject = null;
		SceneMaster.Instance.LoadScene(2);
	}

	public void OptionsButton()
	{
		ChangePanelState(PanelState.OPTIONS);

		//InputManager.Instance.ChangeActiveSelectedObject(3);
	}

	public void HowToPlayButton()
	{
		ChangePanelState(PanelState.HOW_TO_PLAY);

		//InputManager.Instance.ChangeActiveSelectedObject(4);
	}

	public void AudioButton()
	{
		ChangePanelState(PanelState.AUDIO);

		//InputManager.Instance.ChangeActiveSelectedObject(5);
	}

	public void GraphicsButton()
	{
		ChangePanelState(PanelState.GRAPHICS);

		//InputManager.Instance.ChangeActiveSelectedObject(6);
	}

	public void ControlsButton()
	{
		ChangePanelState(PanelState.CONTROLS);

		//InputManager.Instance.ChangeActiveSelectedObject(7);
	}

	public void CreditsButton()
	{
		ChangePanelState(PanelState.CREDITS);
	}

	public void BackToOptionsButton()
	{
		ChangePanelState(PanelState.OPTIONS);

		InputManager.Instance.ChangeToPreviousSelectedObject();
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
		//InputManager.Instance.ChangeActiveSelectedObject(0);
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
