using UnityEngine;

public class PlayerDataManager : SingeltonPersistant<PlayerDataManager>
{
    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    [SerializeField]
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
    public int CurrentlyJoinedPlayers
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    protected override void Awake()
    {
        base.Awake();
        playerDatas = new PlayerData[MAX_PLAYER_NUMBER];
    }

    private void Start()
    {
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
        if (CurrentlyJoinedPlayers <= 0)
            return;

        foreach (var playerData in playerDatas)
        {
            if (playerData.HasJoined)
            {
                var newPlayer = Instantiate(ResourceManager.Instance.GetPrefabByName("Player"));
                newPlayer.GetComponent<Player>().PlayerData = playerData;
            }
        }
    }
}
