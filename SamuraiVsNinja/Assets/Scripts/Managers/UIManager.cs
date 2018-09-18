using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    [SerializeField]
    private Transform HUDCanvas;

    [SerializeField]
    private GameObject lobbyPanel;
    public GameObject LobbyPanel
    {
        get
        {
            return lobbyPanel;
        }
        set
        {
            lobbyPanel = value;
        }
    }

    [SerializeField]
    private readonly Transform matchContainer;
    public Transform MatchContainer
    {
        get
        {
            return matchContainer;
        }
    }

    [SerializeField]
    private readonly GameObject matchButtonPrefab;
    public GameObject MatchButtonPrefab
    {
        get
        {
            return matchButtonPrefab;
        }
    }

    [SerializeField]
    private Button leaveMatchButton;
    public Button LeaveMatchButtonObject
    {
        get
        {
            return leaveMatchButton;
        }
        set
        {
            leaveMatchButton = value;
        }
    }

    [SerializeField]
    private Text statusText;
    public string StatusText
    {
        get
        {
            return statusText.text;
        }
        set
        {
            statusText.text = value;
        }
    }

    [SerializeField]
    private Text debugText;
    public string DebugText
    {
        get
        {
            return debugText.text;
        }
        set
        {
            debugText.text = value;
        }
    }

    [SerializeField]
    private Text timerText;
    public Text TimerText
    {
        get
        {
            return timerText;
        }
        set
        {
            timerText = value;
        }
    }

    public Color TimerTextColor
    {
        get
        {
            return TimerText.color;
        }
    }

    private void Start()
    {
        HUDCanvas.gameObject.SetActive(true);
        LobbyPanel.SetActive(true);
        LeaveMatchButtonObject.gameObject.SetActive(false);
    }

    public void ModifyTimerText(string message, Color newColor)
    {
        TimerText.text = message;
        TimerText.color = newColor;
    }

    #region UI_BUTTONS

    public void ChangeMachNameButton(string newMatchName)
    {
        MainNetworkManager.Instance.MatchName = newMatchName;
    }

    public void ChangeMachSizeButton(int newMatchSize)
    {
        newMatchSize = newMatchSize == 0 ? 2 : 4;
        MainNetworkManager.Instance.MatchSize = (uint)newMatchSize;
    }

    public void CreateMatchButton()
    {
        MainNetworkManager.Instance.CreateMatch();
    }

    public void RefreshMatchListButton()
    {
        MainNetworkManager.Instance.RefreshMatches();
    }

    public void LeaveMatchButton()
    {
        MainNetworkManager.Instance.LeaveMatch();
        LobbyPanel.SetActive(true);
        LeaveMatchButtonObject.gameObject.SetActive(false);
    }

    public void BackToMenuButton()
    {
        SceneMaster.Instance.LoadScene(0);
    }

    #endregion UI_BUTTONS
}
