using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singelton<MainMenuManager>
{
	public string JoinedPlayersText
	{
		get
		{
			return joinedPlayerText.text;
		}
		set
		{
			joinedPlayerText.text = value;
		}
	}
	public bool CanStart
	{
		get
		{
			return startButton.interactable = InputManager.Instance.CurrentlyJoinedPlayers > 0;
		}
	}


	private GameObject menuCanvasGameObject;
	private GameObject mainButtonPanel;
	private GameObject characterSelectPanel;
	private GameObject characterSelectContainer;

	private GameObject panels;

	private JoinField[] joinFields;
	private Button startButton;

	private Text joinedPlayerText;

	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";

	private void Awake()
	{
		menuCanvasGameObject = GameObject.Find("MainMenuCanvas");

		panels = menuCanvasGameObject.transform.Find("Panels").gameObject;
		mainButtonPanel = panels.transform.Find("MainButtonPanel").gameObject;
		characterSelectPanel = panels.transform.Find("CharacterSelectPanel").gameObject;
		characterSelectContainer = characterSelectPanel.transform.Find("CharacterSelectContainer").gameObject;

		joinFields = GetJoinFields();

		joinedPlayerText = characterSelectPanel.transform.Find("JoinedPlayers").GetComponent<Text>();
		startButton = characterSelectContainer.transform.Find("StartButton").GetComponent<Button>();
		mainMenuCanvasAnimator = menuCanvasGameObject.GetComponent<Animator>();
	}

	private JoinField[] GetJoinFields()
	{
		return characterSelectContainer.GetComponentsInChildren<JoinField>(true);
	}

	private void Start()
	{
		mainButtonPanel.SetActive(true);
		characterSelectPanel.SetActive(false);
	}

	public void PlayerJoinField(int index, string playerName)
	{
		index--;
		joinFields[index].GetComponentInChildren<Text>().text = playerName;
		joinFields[index].GetComponentInChildren<Image>().color = Color.green;

		if (CanStart)
			startButton.interactable = true;
	}

	public void EndCredits()
	{
		if (mainMenuCanvasAnimator.GetCurrentAnimatorStateInfo(0).IsTag(creditsAnimationTag))
		{
			mainMenuCanvasAnimator.Play("Credits", 0, 0.98f);
			return;
		}
	}

	#region UI BUTTONS

	public void PlayButton()
	{
		if (CanStart)
			startButton.interactable = true;

		mainButtonPanel.SetActive(false);
		characterSelectPanel.SetActive(true);

		InputManager.Instance.CanPlayerJoin = true;
	}

	public void StartButton()
	{
		InputManager.Instance.CanPlayerJoin = false;
	   
		SceneMaster.Instance.LoadScene("DevScene - Niko");
	}

	public void OnlineButton()
	{
		SceneMaster.Instance.LoadScene("DevScene - Mathias");
	}

	public void OptionsButton()
	{

	}

	public void CreditsButton()
	{
		mainMenuCanvasAnimator.Play("Credits");
	}

	public void BackToMenuButton()
	{
		characterSelectPanel.SetActive(false);
		mainButtonPanel.SetActive(true);

		InputManager.Instance.CanPlayerJoin = false;
	}

	public void QuitButton()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#endregion UI BUTTONS
}
