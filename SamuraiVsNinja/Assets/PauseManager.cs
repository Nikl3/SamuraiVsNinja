using UnityEngine;

public class PauseManager : MonoBehaviour {
    public Canvas PauseCanvas;


    private void Awake()
    {
        PauseCanvas.GetComponent<Canvas>();

    }

    private void Start()
    {
        PauseCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        GamePaused();

    }

    private void GamePaused()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        //if (Input.GetButtonDown())

    }

    public void ContinueButton()
    {
        PauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


}


