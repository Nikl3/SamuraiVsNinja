﻿using UnityEngine;
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

    public void CreateMatchButton()
    {
        MainNetworkManager.Instance.CreateMatch();
    }

    public void RefreshMatchListButton()
    {
        MainNetworkManager.Instance.RefreshMatches();
        StatusText = "Loading ...";
    }

    public void LeaveMatchButton()
    {
        var matchInfo = MainNetworkManager.Instance.matchInfo;
        MainNetworkManager.Instance.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, MainNetworkManager.Instance.OnDropConnection);
        MainNetworkManager.Instance.StopHost();
        LobbyPanel.SetActive(true);
    }

    #endregion UI_BUTTONS
}
