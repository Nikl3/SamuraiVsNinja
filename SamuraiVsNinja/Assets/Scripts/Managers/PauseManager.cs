using UnityEngine;
using UnityEngine.UI;

public class PauseManager : Singelton<PauseManager>
{
    private GameObject pausePanel;
    private GameObject victoryPanel;
    private Text wienerText;
    private bool isPaused;

    private void Awake()
    {
        pausePanel = transform.Find("PausePanel").gameObject;
        victoryPanel = transform.Find("VictoryPanel").gameObject;
        wienerText = victoryPanel.transform.Find("WienerText").GetComponent<Text>();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel_J1"))
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

    public void VictoryPanel(string wienerName) {
        victoryPanel.SetActive(true);
        wienerText.text = wienerName + "\nYou are THE wiener!";
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
