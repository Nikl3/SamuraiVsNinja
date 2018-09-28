using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_TYPE
{
    NINJA,
    SAMURAI
}

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public RuntimeAnimatorController[] RuntimeAnimatorControllers;
    public Sprite[] PlayerIconSprite;
    public Sprite[] DashIconSprite;
    public Sprite[] ProjectileIconSprite;

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
    private void AddTestPlayers(int testPlayerAmount)
    {
        if (TestPlayerAmount > 0)
        {
            testPlayerAmount = testPlayerAmount > MAX_PLAYER_NUMBER ? MAX_PLAYER_NUMBER : testPlayerAmount;

            for (int i = 0; i < testPlayerAmount; i++)
            {
                PlayerDatas[i].HasJoined = true;
            }
        }                   
    }

    public PlayerData GetPlayerData(int id)
    {
        return PlayerDatas[id];
    }
    //public void ClearJoinedPlayers()
    //{
    //    CurrentlyJoinedPlayers.Clear();
    //}
    public void PlayerJoin(int playerID)
    {
        if (!PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = true;

            return;
        }
    }
    public void PlayerUnjoin(int playerID)
    {
        if (PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = false;          
            return;
        }
    } 
    public void SpawnPlayers()
    {
        AddTestPlayers(TestPlayerAmount);

        foreach (var playerData in PlayerDatas)
        {
            if (playerData.HasJoined)
            {
                var newPlayer = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0), LevelManager.Instance.GetSpawnPoint(playerData.ID - 1), Quaternion.identity).GetComponent<Player>();
                CurrentlyJoinedPlayers.Add(newPlayer);
                var newPlayerInfo = Instantiate(
                    ResourceManager.Instance.GetPrefabByIndex(4, 1).GetComponent<PlayerInfo>());
                
                var newPlayerEG = Instantiate(EGPrefab);
                newPlayerEG.transform.SetParent(EGPanel);
                newPlayerEG.transform.localScale = Vector2.one;

                newPlayerInfo.EGS = newPlayerEG.GetComponent<EndGameStats>();
            
                // var newPlayer = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(0, 0), LevelManager.Instance.GetSpawnPoint(playerData.ID - 1)).GetComponent<Player>();

                newPlayer.Initialize(playerData, newPlayerInfo, playerData.PlayerType == PLAYER_TYPE.NINJA ? RuntimeAnimatorControllers[0] : RuntimeAnimatorControllers[1]);
                CameraEngine.Instance.AddTarget(newPlayer.transform);
                newPlayer.ChangePlayerState(PlayerState.RESPAWN, true);
            }
        }
    }  
}
