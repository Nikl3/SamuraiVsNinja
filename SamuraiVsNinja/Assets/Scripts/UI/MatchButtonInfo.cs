using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
//using System;

public class MatchButtonInfo : MonoBehaviour
{
    public delegate void OnJoinMatch(MatchInfoSnapshot matchInfoSnapshot);
    private OnJoinMatch JoinMatchCallback;
    //public event Action<MatchInfoSnapshot match> OnMatchJoined;
    private Text matchInfoText;
    private MatchInfoSnapshot match;

    private void Awake()
    {
        matchInfoText = GetComponentInChildren<Text>();
    }

    public void Initialize(MatchInfoSnapshot match, OnJoinMatch JoinMatchCallback)
    {
        this.match = match;
        this.JoinMatchCallback = JoinMatchCallback;
        matchInfoText.text = match.name + " ( " + match.currentSize + " / "+ match.maxSize + " )";
    }

    public void JoinMatch()
    {
        JoinMatchCallback.Invoke(match);
        UIManager.Instance.LobbyPanel.SetActive(false);
    }
}
