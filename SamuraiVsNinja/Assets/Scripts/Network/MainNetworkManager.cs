using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MainNetworkManager : NetworkManager
{
    #region VARIABLES

    public static MainNetworkManager Instance { get; private set; }

    private List<GameObject> matchList = new List<GameObject>();

    public uint MatchSize
    {
        get
        {
            return _matchSize;
        }
        set
        {
            _matchSize = value;
        }
    }
    private uint _matchSize = 2;
    public string MatchName
    {
        get
        {
            return _matchName;
        }
        set
        {
            _matchName = value;
        }
    }
    private string _matchName;
    private bool matchAdvertise = true;
    private string matchPassword = "";
    private string publicClientAddress = "";
    private string privateClientAddress = "";
    private int eloScoreForMatch = 0;
    private int requestDomain = 0;

    #endregion VARIABLES

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        CheckMachMakerState();

        RefreshMatches();
    }

    /// <summary>
    /// Since we have only one scene for now, so the match maker gets set to "NULL".
    /// We could run CheckMachMakerState() at start of "Lobby" scene to prevent null reference exceotions.
    /// </summary>
    private void CheckMachMakerState()
    {
        if (matchMaker == null)
            StartMatchMaker();
    }

    private void ClearMatchList()
    {
        for (int i = 0; i < matchList.Count; i++)
        {
            Destroy(matchList[i]);
        }

        matchList.Clear();
    }

    public void CreateMatch()
    {
        CheckMachMakerState();

        if (MatchName != null)
        {
            Debug.Log("Create match with name: " + MatchName + " and has size of: " + MatchSize + " players");
            matchMaker.CreateMatch(MatchName, MatchSize, matchAdvertise, matchPassword, publicClientAddress, privateClientAddress, eloScoreForMatch, requestDomain, OnMatchCreate);

            UIManager.Instance.LobbyPanel.SetActive(false);
            UIManager.Instance.LeaveMatchButtonObject.gameObject.SetActive(true);
            UIManager.Instance.StatusText = "";
        }
        else
        {
            UIManager.Instance.StatusText = "Match need to have a name";
            Debug.LogError("Can not create a match, because MatchName can not be null!");
        }     
    }

    public void RefreshMatches()
    {
        CheckMachMakerState();

        ClearMatchList();
        matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
        UIManager.Instance.StatusText = "Refreshing...";
    }

    public void LeaveMatch()
    {
        // matches does not get destroied perectly so we can join unhosted room
        // somethimes. There will be error when we try to drop connection.
        // Match info could be null at this point.
        MatchInfo _matchInfo = singleton.matchInfo;
        if (_matchInfo != null)
        {
            matchMaker.DropConnection(_matchInfo.networkId, _matchInfo.nodeId, 0, OnDropConnection);
            singleton.StopHost();
            RefreshMatches();
        }       
    }

    public void JoinMatch(MatchInfoSnapshot match)
    {
        matchMaker.JoinMatch(match.networkId, matchPassword, publicClientAddress, privateClientAddress, eloScoreForMatch, requestDomain, OnMatchJoined);
        ClearMatchList();
        UIManager.Instance.StatusText = "Joining...";
        UIManager.Instance.LeaveMatchButtonObject.gameObject.SetActive(true);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        UIManager.Instance.StatusText = "";

        if (success == false || matches == null)
        {
            Debug.LogError("Could not find matches!");
        }

        foreach (MatchInfoSnapshot match in matches)
        {
            GameObject matchListItemGameObject = Instantiate(UIManager.Instance.MatchButtonPrefab);
        
            matchListItemGameObject.transform.SetParent(UIManager.Instance.MatchContainer);
            matchListItemGameObject.transform.localScale = Vector3.one;
            MatchButtonInfo matchButtonInfo = matchListItemGameObject.GetComponent<MatchButtonInfo>();
            if (matchButtonInfo != null)
            {
                matchButtonInfo.Initialize(match, JoinMatch);
            }

            matchList.Add(matchListItemGameObject);
        }

        if (matches.Count == 0)
        {
            UIManager.Instance.StatusText = "No matches available";
        }
    }
}
