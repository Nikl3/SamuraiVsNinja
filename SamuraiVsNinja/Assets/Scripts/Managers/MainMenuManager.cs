using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singelton<MainMenuManager>
{
	#region VARIABLES

	private GameObject menuCanvasGameObject;
	private GameObject mainButtonPanel;
	private GameObject characterSelectPanel;
	private GameObject characterSelectContainer;
	private GameObject panels;

	private JoinField[] joinFields;
	private Color fieldColor = Color.green;

	private Button startButton;
	private Text joinedPlayerText;

	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";
	private string offlineScene = "DevScene - Niko";
	private string onlineScene = "DevScene - Mathias";

	#endregion VARIABLES

	#region PROPERTIES

	#endregion PROPERTIES

	private bool CanStart()
	{
		return startButton.interactable = PlayerDataManager.Instance.CurrentJoinedPlayers == 4 || PlayerDataManager.Instance.CurrentJoinedPlayers == 2 ? true : false;
	}

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

	private void Start()
	{
		mainButtonPanel.SetActive(true);
		characterSelectPanel.SetActive(false);
		CanStart();
	}

	private JoinField[] GetJoinFields()
	{
		return characterSelectContainer.GetComponentsInChildren<JoinField>(true);
	}

	public void SetJoinField(int playerIndex, string joinedPlayerName)
	{
		for (int i = 0; i < joinFields.Length; i++)
		{
			if (i == playerIndex)
			{
				joinFields[i].ChangeJoinFieldVisuals(joinedPlayerName.ToUpper(), fieldColor);
			}
		}

		joinedPlayerText.text = "PLAYERS " + (playerIndex + 1) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
		CanStart();
	}

    public void UnSetJoinField(int playerIndex, string joinedPlayerName)
    {
        for (int i = 0; i < joinFields.Length; i++)
        {
            if (i == playerIndex)
            {
                joinFields[i].UnChangeJoinFieldVisuals();
            }
        }

        joinedPlayerText.text = "PLAYERS " + (playerIndex) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
        CanStart();
    }

    public void UnSetAllJoinField(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			joinFields[i].UnChangeJoinFieldVisuals();
		}

		joinedPlayerText.text = "PLAYERS 0 / " + PlayerDataManager.Instance.MaxPlayerNumber;
		CanStart();
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
		mainButtonPanel.SetActive(false);
		characterSelectPanel.SetActive(true);

		PlayerDataManager.Instance.CanJoin = true;
	}

	public void StartButton()
	{
		SceneMaster.Instance.LoadScene(offlineScene);
		PlayerDataManager.Instance.CanJoin = false;
	}

	public void OnlineButton()
	{
		SceneMaster.Instance.LoadScene(onlineScene);
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

		PlayerDataManager.Instance.CanJoin = false;
		PlayerDataManager.Instance.ClearPlayersData();

		UnSetAllJoinField(joinFields.Length);
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
