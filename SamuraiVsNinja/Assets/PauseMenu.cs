using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseMenuUI; 

  
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PauseMenuUI.SetActive(true);
        }
        
	}

    public void PlayGame()
    {
        SceneManager.GetActiveScene();
        Time.timeScale = 1;
        PauseMenuUI.SetActive(false);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
