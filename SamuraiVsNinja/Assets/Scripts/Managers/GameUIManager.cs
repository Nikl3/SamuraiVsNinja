using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : Singelton<GameUIManager>
{
    private GameObject pausePanel;
    private GameObject victoryPanel;
    private Text wienerText;
    private bool isPaused;

    private void Awake()
    {
        pausePanel = transform.Find("PausePanel").gameObject;
        victoryPanel = transform.Find("VictoryPanel").gameObject;
        wienerText = victoryPanel.transform.Find("WinnerText").GetComponent<Text>();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.Start_ButtonDown(1))
        {
            if (isPaused)
            {
                ContinueButton();
            }
            else
            {
                Paused();
            }
        }
    }

    public void VictoryPanel(string winnerName)
    {
        victoryPanel.SetActive(true);
        wienerText.text = winnerName + "\nYou are THE winner!";
        Time.timeScale = 0;
    }

    private void Paused()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void RestartGameButton()
    {
        SceneMaster.Instance.LoadScene(1);
    }

    public void OptionsButton()
    {

    }

    public void ExitGameButton()
    {
        SceneMaster.Instance.LoadScene(0);
    }
}
