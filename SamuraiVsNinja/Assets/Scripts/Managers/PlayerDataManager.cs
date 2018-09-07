using UnityEngine;

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public int TestPlayerAmount;

    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;
    private PlayerData[] playerDatas;
    private bool canJoin = false;

    #endregion VARIABLES

    #region PROPERTIES

    public int MaxPlayerNumber
    {
        get
        {
            return MAX_PLAYER_NUMBER;
        }
    }

    public int CurrentlyJoinedPlayers
    {
        get;
        private set;
    }

    public bool CanJoin
    {
        get
        {
            return canJoin;
        }
        set
        {
            canJoin = value;
        }
    } 

    #endregion PROPERTIES

    private void Awake()
    {
        CreatePlayerDatas();
    }

    private void CreatePlayerDatas()
    {
        playerDatas = new PlayerData[MAX_PLAYER_NUMBER];

        for (int i = 0; i < playerDatas.Length; i++)
        {
            playerDatas[i] = new PlayerData(i + 1)
            {
               
            };
        }
    }

    public void PlayerJoin(int playerID)
    {
        if (!playerDatas[playerID].HasJoined)
        {
            playerDatas[playerID].HasJoined = true;
            CurrentlyJoinedPlayers++;
            return;
        }
    }

    public void PlayerUnjoin(int playerID)
    {
        if (playerDatas[playerID].HasJoined)
        {
            playerDatas[playerID].HasJoined = false;
            CurrentlyJoinedPlayers--;
            return;
        }
    } 

    public void ClearPlayerDataIndex()
    {
        CurrentlyJoinedPlayers = 0;
        foreach (var data in playerDatas)
        {
            data.HasJoined = false;
        }
    }

    public void SpawnPlayers()
    {
        AddTestPlayers(TestPlayerAmount);

        foreach (var playerData in playerDatas)
        {       
            if (playerData.HasJoined)
            {
                var newPlayer = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0).GetComponent<Player>(),
                    GameManager.Instance.RandomSpawnPoint(), Quaternion.identity);
                var newPlayerInfo = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 1).GetComponent<PlayerInfo>());

                newPlayer.Initialize(playerData, newPlayerInfo);

                CameraEngine.Instance.AddTarget(newPlayer.transform);
            }                     
        }
    }


    private void AddTestPlayers(int testPlayerAmount)
    {
        testPlayerAmount = testPlayerAmount > MAX_PLAYER_NUMBER ? MAX_PLAYER_NUMBER : testPlayerAmount;

        for (int i = 0; i < testPlayerAmount; i++)
        {
            playerDatas[i].HasJoined = true;
        }
    }
}
