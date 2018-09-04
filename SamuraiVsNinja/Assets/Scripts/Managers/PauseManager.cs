using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private GameObject pausePanel;
    private bool isPaused;

    private void Awake()
    {
        pausePanel = transform.Find("PausePanel").gameObject;
    }

    private void Start()
    {
        pausePanel.SetActive(false);
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
