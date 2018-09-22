using UnityEngine;

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public int TestPlayerAmount;

    #region VARIABLES

    private GameObject playerPrefab;
    private GameObject playerInfoPrefab;

    private const int MAX_PLAYER_NUMBER = 4;
    private PlayerData[] playerDatas;

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
            playerDatas[i] = new PlayerData(i + 1, CreatePlayerColor(i + 1))
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
        if (!playerDatas[playerID - 1].HasJoined)
        {
            playerDatas[playerID - 1].HasJoined = true;
            CurrentlyJoinedPlayers++;
            return;
        }
    }

    public void PlayerUnjoin(int playerID)
    {
        if (playerDatas[playerID - 1].HasJoined)
        {
            playerDatas[playerID - 1].HasJoined = false;
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

    public PlayerData GetPlayerData(int id)
    {
        return playerDatas[id];
    }

    public void SpawnPlayers(Transform parent)
    {
        AddTestPlayers(TestPlayerAmount);

        foreach (var playerData in playerDatas)
        {
            if (playerData.HasJoined)
            {
                //var newPlayerGameObject = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0),
                //    LevelManager.Instance.RandomSpawnPoint(),
                //    Quaternion.identity);

                var newPlayerGameObject = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(0, 0));
                newPlayerGameObject.transform.SetPositionAndRotation(LevelManager.Instance.RandomSpawnPoint(), Quaternion.identity);
                newPlayerGameObject.transform.SetParent(parent);

                var newPlayerInfo = Instantiate(
                    ResourceManager.Instance.GetPrefabByIndex(4, 1).GetComponent<PlayerInfo>());

                var newPlayer = newPlayerGameObject.GetComponent<Player>();
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
            playerDatas[i].HasJoined = true;
        }
    }
}
