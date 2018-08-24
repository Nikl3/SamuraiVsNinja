using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public int ID
    {
        get
        {
            return id;
        }
    }
    public string PlayerName
    {
        get
        {
           return playerName;
        }
    }
    public bool HasAssigned
    {
        get
        {
            return hasAssigned;
        }
    }

    private int id;
    private string playerName;
    private bool hasAssigned;

    public PlayerData(int id)
    {
        this.id = id;
        playerName = "LocalPlayer " + id;
        hasAssigned = true;
    }
}

public class InputManager : SingeltonPersistant<InputManager>
{
    #region VARIABLES

    private const int MAX_PLAYER_COUNT = 4;
    private int currentlyJoinedPlayers = 0;

    private bool canPlayerJoin = false;

    private List<PlayerData> playerDatas = new List<PlayerData>();

    #endregion VARIABLES

    #region PROPERTIES

    public bool CanPlayerJoin
    {
        get
        {
            return canPlayerJoin;
        }
        set
        {
            canPlayerJoin = value;
        }
    }
    public int CurrentlyJoinedPlayers
    {
        get
        {
            return currentlyJoinedPlayers;
        }     
    }

    #endregion PROPERTIES

    protected override void Awake()
    {
        base.Awake();
    }

    public PlayerData GetCorrectPlayerData(string playerName)
    {
        Debug.Log(playerName);

        foreach(var data in playerDatas)
        {
            if(data.PlayerName == playerName)
            {
                return data;
            }
        }

        Debug.LogError("We did not have correct payer data...");
        return new PlayerData();
    }

    private void OnPlayerJoined()
    {
        if(playerDatas == null || playerDatas.Count > 0)
        {
            foreach (var data in playerDatas)
            {
                if(currentlyJoinedPlayers == data.ID && data.HasAssigned)
                {
                    Debug.LogError("Already assigned player " + data.ID);
                    return;
                }
            }
        }

        currentlyJoinedPlayers++;
        PlayerData playerData = new PlayerData(currentlyJoinedPlayers)
        {

        };

        Debug.LogError("Player " + playerData.ID + " joined!");

        playerDatas.Add(playerData);

        MainMenuManager.Instance.PlayerJoinField(playerData.ID, "P " + playerData.ID);

        MainMenuManager.Instance.JoinedPlayersText = "( " + currentlyJoinedPlayers + " / " + MAX_PLAYER_COUNT + " )";
    }

    private void OnPlayerUnJoined()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            MainMenuManager.Instance.EndCredits();
        }

        if (!canPlayerJoin)
            return;

        if (Input.GetButtonDown("Action"))
        {
            //Debug.LogError("Player joined with keyboard!");
            OnPlayerJoined();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            OnPlayerUnJoined();
        }
    }
}
