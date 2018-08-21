using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject PauseMenuUI; 
 
	private void Update ()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Time.timeScale = 0;
			PauseMenuUI.SetActive(true);
		}	
	}

	public void PlayGame()
	{
		Time.timeScale = 1;
		PauseMenuUI.SetActive(false);
	}

	public void ToMenu()
	{
	   // SceneManager.LoadScene("MenuScene");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
