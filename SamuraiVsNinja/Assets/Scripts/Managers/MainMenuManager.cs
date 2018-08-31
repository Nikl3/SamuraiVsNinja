using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singelton<MainMenuManager>
{
	#region VARIABLES

	private GameObject menuCanvasGameObject;
    private GameObject panels;
    private GameObject mainButtonPanel;
    private GameObject optionsPanel;
    private GameObject howToPlayPanel;
    private GameObject audioPanel;
    private GameObject graphicsPanel;
    private GameObject controlsPanel;
    private GameObject characterSelectPanel;
    private GameObject characterSelectContainer;

	private JoinField[] joinFields;
	private Color fieldColor = Color.green;

	private Button startButton;
	private Text joinedPlayerText;

	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";
	//private string offlineScene = "DevScene - Niko";
	//private string onlineScene = "DevScene - Mathias";

	#endregion VARIABLES

	#region PROPERTIES

	#endregion PROPERTIES

	private bool CanStart()
	{
		return startButton.interactable = PlayerDataManager.Instance.CurrentlyJoinedPlayers == 4 || PlayerDataManager.Instance.CurrentlyJoinedPlayers == 2 ? true : false;
	}

	private void Awake()
	{
		menuCanvasGameObject = GameObject.Find("MainMenuCanvas");

		panels = menuCanvasGameObject.transform.Find("Panels").gameObject;
		mainButtonPanel = panels.transform.Find("MainButtonPanel").gameObject;
        optionsPanel = panels.transform.Find("OptionsPanel").gameObject;
        howToPlayPanel = panels.transform.Find("HowToPlayPanel").gameObject;
        audioPanel = panels.transform.Find("AudioPanel").gameObject;
        graphicsPanel = panels.transform.Find("GraphicsPanel").gameObject;
        controlsPanel = panels.transform.Find("ControlsPanel").gameObject;
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
        optionsPanel.SetActive(false);

		CanStart();
	}

	private JoinField[] GetJoinFields()
	{
		return characterSelectContainer.GetComponentsInChildren<JoinField>(true);
	}

	public void SetJoinField(int playerID)
	{
        joinFields[playerID - 1].ChangeJoinFieldVisuals(playerID, fieldColor);
		joinedPlayerText.text = "PLAYERS " + (playerID) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
		CanStart();
	}

    public void UnSetJoinField(int playerID)
    {
        for (int i = 0; i < joinFields.Length; i++)
        {

            joinFields[i].UnChangeJoinFieldVisuals();
            
        }

        joinedPlayerText.text = "PLAYERS " + (PlayerDataManager.Instance.CurrentlyJoinedPlayers) + " / " + PlayerDataManager.Instance.MaxPlayerNumber;
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
        PlayerDataManager.Instance.CanJoin = false;
        SceneMaster.Instance.LoadScene(1);
	}

	public void OnlineButton()
	{
		SceneMaster.Instance.LoadScene(2);
	}

	public void OptionsButton()
	{
        optionsPanel.SetActive(true);
        mainButtonPanel.SetActive(false);
    }

    public void HowToPlayButton()
    {
        howToPlayPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void AudioButton()
    {
        audioPanel.SetActive(true);
        optionsPanel.SetActive(false);

    }

    public void GraphicsButton()
    {
        graphicsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ControlsButton()
    {
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void CreditsButton()
	{
		mainMenuCanvasAnimator.Play("Credits");
	}

    public void BackToOptionsButton()
    {
        optionsPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);
       
    }

	public void BackToMenuButton()
	{
        if (characterSelectPanel.activeSelf)
        {
            PlayerDataManager.Instance.CanJoin = false;
            PlayerDataManager.Instance.ClearPlayerDataIndex();
            UnSetAllJoinField(joinFields.Length);
            characterSelectPanel.SetActive(false);
        }

        optionsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        mainButtonPanel.SetActive(true);
	}

	public void QuitButton()
	{
        SceneMaster.Instance.ExitGame(() => OnQuit());
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    #endregion UI BUTTONS
}
