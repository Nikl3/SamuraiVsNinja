using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public int TestPlayerAmount;
    public GameObject EGPrefab;
    public Transform EGPanel;

    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    #endregion VARIABLES

    #region PROPERTIES

    public PlayerData[] PlayerDatas
    {
        get;
        private set;
    }
    public List<Player> CurrentlyJoinedPlayers
    {
        get;
        private set;
    }

    public int MaxPlayerNumber
    {
        get
        {
            return MAX_PLAYER_NUMBER;
        }
    }

    //public int CurrentlyJoinedPlayersIndex
    //{
    //    get;
    //    private set;
    //}

    #endregion PROPERTIES

    private void Awake()
    {
        CreatePlayerDatas();        
    }

    private void CreatePlayerDatas()
    {
        CurrentlyJoinedPlayers = new List<Player>();
        PlayerDatas = new PlayerData[MAX_PLAYER_NUMBER];

        for (int i = 0; i < PlayerDatas.Length; i++)
        {
            PlayerDatas[i] = new PlayerData(i + 1, CreatePlayerColor(i + 1))
            {

            };
        }
    }

    private Color CreatePlayerColor(int playerID)
    {
        switch (playerID)
        {
            case 1:
                return Color.blue;

            case 2:
                return Color.red;

            case 3:
                return Color.green;

            case 4:
                return Color.yellow;

            default:
                return Color.magenta;
        }
    }

    public void PlayerJoin(int playerID)
    {
        if (!PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = true;
            //CurrentlyJoinedPlayersIndex++;
            return;
        }
    }

    public void PlayerUnjoin(int playerID)
    {
        if (PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = false;
            //CurrentlyJoinedPlayers.RemoveAt(playerID - 1);
            //CurrentlyJoinedPlayersIndex--;
            return;
        }
    } 

    //public void ClearPlayerDataIndex()
    //{
    //    CurrentlyJoinedPlayersIndex = 0;
    //    foreach (var data in PlayerDatas)
    //    {
    //        data.HasJoined = false;
    //    }
    //}

    public PlayerData GetPlayerData(int id)
    {
        return PlayerDatas[id];
    }

    public void SpawnPlayers(Transform parent)
    {
        AddTestPlayers(TestPlayerAmount);

        foreach (var playerData in PlayerDatas)
        {
            if (playerData.HasJoined)
            {
                var newPlayerGameObject = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0));
                newPlayerGameObject.transform.SetPositionAndRotation(LevelManager.Instance.RandomSpawnPoint(), Quaternion.identity);
                newPlayerGameObject.transform.SetParent(parent);

                var newPlayerInfo = Instantiate(
                    ResourceManager.Instance.GetPrefabByIndex(4, 1).GetComponent<PlayerInfo>());
                
                var newPlayerEG = Instantiate(EGPrefab);
                newPlayerEG.transform.SetParent(EGPanel);
                newPlayerEG.transform.localScale = Vector2.one;

                newPlayerInfo.EGS = newPlayerEG.GetComponent<EndGameStats>();

                var newPlayer = newPlayerGameObject.GetComponent<Player>();
                CurrentlyJoinedPlayers.Add(newPlayer);

                newPlayer.Initialize(playerData, newPlayerInfo);
                newPlayer.ChangePlayerState(PlayerState.INVINCIBILITY);

                CameraEngine.Instance.AddTarget(newPlayer.transform);
      
                Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 1), newPlayer.transform.position, Quaternion.identity);
            }
        }
    }

    private void AddTestPlayers(int testPlayerAmount)
    {
        testPlayerAmount = testPlayerAmount > MAX_PLAYER_NUMBER ? MAX_PLAYER_NUMBER : testPlayerAmount;

        for (int i = 0; i < testPlayerAmount; i++)
        {
            PlayerDatas[i].HasJoined = true;
        }
    }
}
