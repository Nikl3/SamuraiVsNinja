using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MainNetworkManager : NetworkManager
{
    #region VARIABLES

    public static MainNetworkManager Instance { get; private set; }

    private List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();

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

    private void OnMatchCreated(bool success, string extendedinfo, MatchInfo responsedata)
    {
        base.StartHost(responsedata);
        RefreshMatches();

        UIManager.Instance.LobbyPanel.SetActive(false);
    }

    public void RefreshMatches()
    {
        if (matchMaker == null)
            StartMatchMaker();

        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListMatchesComplete);
    }

    private void HandleListMatchesComplete(bool success, string extendedinfo, List<MatchInfoSnapshot> responsedata)
    {
        AvailableMatchesList.HandleNewMatchList(responsedata);
    }

    public void JoinMatch(MatchInfoSnapshot match)
    {
        if (matchMaker == null)
            StartMatchMaker();

        matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, HandleJoinedMatch);

        UIManager.Instance.LobbyPanel.SetActive(false);
    }

    private void HandleJoinedMatch(bool success, string extendedinfo, MatchInfo responsedata)
    {
        StartClient(responsedata);
    }

    private void Awake()
    {
        if (Instance == null)
        Instance = this;     
    }

    private void Start()
    {
        RefreshMatches();
    }

    public void CreateMatch()
    {
        if (Instance.matchMaker == null)
        {
            StartMatchMaker();
        }

        if (_matchName != null && _matchName != "")
        {
            Debug.Log("Creating match: " + _matchName + " with match for " + _matchSize + " players!");
            Instance.matchMaker.CreateMatch
                (
                _matchName,
                _matchSize,
                matchAdvertise,
                matchPassword,
                publicClientAddress,
                privateClientAddress,
                eloScoreForMatch,
                requestDomain,
                OnMatchCreated
                );
        }
        else
        {
            Debug.LogError("Match name cannot be null or an empty string");
        }
    }

    private void HandelListMatchesComplete(bool success, string extendedInfo, List<MatchInfoSnapshot> matchesList)
    {
        AvailableMatchesList.HandleNewMatchList(matchesList);
    }

    #region NETWORK_VIRTUAL_EVENT_FUNCTIONS

    #region HOST_FUNCTIONS

    public override NetworkClient StartHost()
    {
        return base.StartHost();
    }

    public override NetworkClient StartHost(ConnectionConfig config, int maxConnections)
    {
        return base.StartHost(config, maxConnections);
    }

    public override NetworkClient StartHost(MatchInfo info)
    {
        return base.StartHost(info);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }

    #endregion HOST_FUNCTIONS

    #region CLIENT_FUNCTIONS

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        Debug.Log("OnStartClient: " + client.ToString());
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        base.OnClientNotReady(conn);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    #endregion CLIENT_FUNCTIONS

    #region MATCH_FUNCTIONS

    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        base.OnDestroyMatch(success, extendedInfo);
        UIManager.Instance.DebugText = "OnDestroyMatch";
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        UIManager.Instance.DebugText = "OnMatchCreate: " + matchInfo.networkId + " created";
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        UIManager.Instance.DebugText = "OnMatchJoined: " + matchInfo.nodeId;
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);
    }

    public override void OnSetMatchAttributes(bool success, string extendedInfo)
    {
        base.OnSetMatchAttributes(success, extendedInfo);
    }

    #endregion MATCH_FUNCTIONS

    #region SERVER_FUNCTIONS

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log("OnServerAddPlayer: Player " + playerControllerId + " added");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        Debug.Log("OnServerAddPlayer: Player " + playerControllerId + " added");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        Debug.LogError("OnServerRemovePlayer: Player " + player + " removed");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }


    #endregion SERVER_FUNCTIONS

    #region OTHER_NETWORK_FUNCTIONS

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
    }

    #endregion OTHER_NETWORK_FUNCTIONS

    #endregion NETWORK_VIRTUAL_EVENT_FUNCTIONS
}
