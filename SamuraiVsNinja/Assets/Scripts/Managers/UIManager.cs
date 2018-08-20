using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
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
    private GameObject matchButtonPrefab;
    public GameObject MatchButtonPrefab
    {
        get
        {
            return matchButtonPrefab;
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

    #region UI_BUTTONS

    public void ChangeMachNameButton(string newMatchName)
    {
        MainNetworkManager.Instance.MatchName = newMatchName;
    }

    public void CreateMatchButton()
    {
        MainNetworkManager.Instance.CreateMatch();
    }

    public void RefreshMatchListButton()
    {
        MainNetworkManager.Instance.RefreshMatches();
        StatusText = "Loading ...";
    }

    #endregion UI_BUTTONS
}
