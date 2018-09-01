using UnityEngine;

public class PlayerDataManager : SingeltonPersistant<PlayerDataManager>
{
    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    [SerializeField]
    private PlayerData[] playerDatas;
    // !!!!!!
    private int currentlyJoinedPlayers = 4;

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
        get
        {
            return currentlyJoinedPlayers;
        }
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

    protected override void Awake()
    {
        base.Awake();
        playerDatas = new PlayerData[MAX_PLAYER_NUMBER];

        for (int i = 0; i < playerDatas.Length; i++)
        {
            playerDatas[i] = new PlayerData(i + 1);
        }
    }

    public void PlayerJoin(int playerID)
    {  
        if (!playerDatas[playerID].HasJoined)
        {
            playerDatas[playerID].HasJoined = true;
            currentlyJoinedPlayers++;
            return;
        }
    }

    public void PlayerUnjoin(int playerID)
    {
        if (playerDatas[playerID].HasJoined)
        {
            playerDatas[playerID].HasJoined = false;
            currentlyJoinedPlayers--;
            return;
        }
    } 

    public void ClearPlayerDataIndex()
    {
        currentlyJoinedPlayers = 0;
        foreach (var data in playerDatas)
        {
            data.HasJoined = false;
        }
    }

    public void SpawnPlayers()
    {
        if (currentlyJoinedPlayers <= 0)
            return;

        foreach (var playerData in playerDatas)
        {
            if (playerData.HasJoined)
            {
                var newPlayer = Instantiate(ResourceManager.Instance.GetPrefabByName("Player").GetComponent<Player>());
                var newPlayerInfo = Instantiate(ResourceManager.Instance.GetPrefabByName("PlayerInfo").GetComponent<PlayerInfo>());

                newPlayer.Initialize(playerData, newPlayerInfo);

                CameraEngine.Instance.AddTarget(newPlayer.transform);
            }
        }
    }
}
