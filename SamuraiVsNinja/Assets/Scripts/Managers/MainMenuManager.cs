using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenuManager : MonoBehaviour
{
	private Animator mainMenuCanvasAnimator;
	private string creditsAnimationTag = "Credits";
    public GameObject CharSelectPanel;

    private void Awake()
	{
		mainMenuCanvasAnimator = GameObject.Find("MainMenuCanvas").GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (mainMenuCanvasAnimator.GetCurrentAnimatorStateInfo(0).IsTag(creditsAnimationTag))
			{
				mainMenuCanvasAnimator.Play("Credits", 0, 0.98f);
				return;
			}
		}
	}

	public void Maps()
	{
		
	}

	#region UI BUTTONS

	public void PlayGameButton()
	{
        //SceneMaster.Instance.LoadScene()
        SceneManager.LoadScene(1);
	}

	public void OnlineButton()
	{
		SceneMaster.Instance.LoadScene(1);
	}

	public void CreditsButton()
	{
		mainMenuCanvasAnimator.Play("Credits");
	}

	public void CharactersButton()
	{
        CharSelectPanel.SetActive(true);

    }

    public void AddPlayer() {
        //show 2player charSelection
        //disable start game until 2player choose char
    }

    public void BackToMenuButton()
	{
        CharSelectPanel.SetActive(false);
		//SceneMaster.Instance.LoadScene(0);
	}

	public void QuitGameButton()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#endregion UI BUTTONS
}
