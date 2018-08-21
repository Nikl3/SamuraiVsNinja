using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private Canvas MenuCanvas;
    private Canvas CreditsCanvas;
    private Canvas CharacterCanvas;
    private Canvas MapCanvas;

    private void Start()
    {
        MenuCanvas = GameObject.Find("MenuCanvas").GetComponent<Canvas>();
        CreditsCanvas = GameObject.Find("CreditsCanvas").GetComponent<Canvas>();
        CharacterCanvas = GameObject.Find("CharacterCanvas").GetComponent<Canvas>();
        MapCanvas = GameObject.Find("MapCanvas").GetComponent<Canvas>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Online()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Credits()
    {
        MenuCanvas.enabled = (false);
        MapCanvas.enabled = (false);
        CharacterCanvas.enabled = (false);
        CreditsCanvas.enabled = (true);
    }

    public void Characters()
    {
        MenuCanvas.enabled = (false);
        MapCanvas.enabled = (false);
        CharacterCanvas.enabled = (true);
        CreditsCanvas.enabled = (false);
    }

    public void Maps()
    {
        MapCanvas.enabled = (true);
        MenuCanvas.enabled = (false);
        CharacterCanvas.enabled = (false);
        CreditsCanvas.enabled = (false);
    }
    
    public void BackToMenu()
    {
        CreditsCanvas.enabled = (false);
        MenuCanvas.enabled = (true);
        MapCanvas.enabled = (false);
        CharacterCanvas.enabled = (false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
