using UnityEditor;
using UnityEngine;


public class MainMenuManager : Singelton<MainMenuManager>
{
	public UIPanel CurrentPanel;

	#region VARIABLES




	private GameObject menuCanvasGameObject;
	private GameObject panels;
	

	private UIPanel mainMenuPanel;
	private UIPanel characterSelectPanel;
	private UIPanel optionsPanel;
	private UIPanel creditsPanel;
	private UIPanel howToPlayPanel;
	private UIPanel audioPanel;
	private UIPanel graphicsPanel;
	private UIPanel controlsPanel;




	

	public Animator MainMenuCanvasAnimator;
	

	#endregion VARIABLES

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

		MainMenuCanvasAnimator = menuCanvasGameObject.GetComponent<Animator>();
	}

	private void Awake()
	{
		Initialize();
	}

	private void Start()
	{
		TriggerOpenBehaviour(mainMenuPanel);
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

	#region UI BUTTONS

	public void PlayButton()
	{
		TriggerOpenBehaviour(characterSelectPanel);
	}

	public void StartButton()
	{
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
		SceneMaster.Instance.ExitGame(() => OnQuit());
	}

	#endregion UI BUTTONS	
}
