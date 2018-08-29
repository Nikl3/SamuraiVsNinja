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

    private void Update()
    {
        if (!canJoin)
            return;

        if (Input.GetButtonDown("Action_J1"))
        {
            print("ACTION_J1");

            if (!playerDatas[0].HasJoined)
            {
                playerDatas[0].HasJoined = true;
                CurrentlyJoinedPlayers++;
                PlayerJoin(playerDatas[0].ID);
                return;
            }       
        }

        if (Input.GetButtonDown("Action_J2"))
        {
            print("ACTION_J2");
            if (!playerDatas[1].HasJoined)
            {
                playerDatas[1].HasJoined = true;
                CurrentlyJoinedPlayers++;
                PlayerJoin(playerDatas[1].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Action_J3"))
        {
            print("ACTION_J3");
            if (!playerDatas[2].HasJoined)
            {
                playerDatas[2].HasJoined = true;
                CurrentlyJoinedPlayers++;
                PlayerJoin(playerDatas[2].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Action_J4"))
        {
            if (!playerDatas[3].HasJoined)
            {
                playerDatas[3].HasJoined = true;
                CurrentlyJoinedPlayers++;
                PlayerJoin(playerDatas[3].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Cancel_J1"))
        {
            print("Cancel_J1");
            if (playerDatas[0].HasJoined)
            {
                playerDatas[0].HasJoined = false;
                CurrentlyJoinedPlayers--;
                PlayerUnjoin(playerDatas[0].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Cancel_J2"))
        {
            print("Cancel_J2");
            if (playerDatas[1].HasJoined)
            {
                playerDatas[1].HasJoined = false;
                CurrentlyJoinedPlayers--;
                PlayerUnjoin(playerDatas[1].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Cancel_J3"))
        {
            print("Cancel_J3");
            if (playerDatas[2].HasJoined)
            {
                playerDatas[2].HasJoined = false;
                CurrentlyJoinedPlayers--;
                PlayerUnjoin(playerDatas[2].ID);
                return;
            }
        }

        if (Input.GetButtonDown("Cancel_J4"))
        {
            print("Cancel_J4");
            if (playerDatas[3].HasJoined)
            {
                playerDatas[3].HasJoined = false;
                CurrentlyJoinedPlayers--;
                PlayerUnjoin(playerDatas[3].ID);
                return;
            }
        }
    }

    private void PlayerJoin(int playerID)
    {
        MainMenuManager.Instance.SetJoinField(playerID);    
    }

    private void PlayerUnjoin(int playerID)
    {
        MainMenuManager.Instance.UnSetJoinField(playerID);
    } 

    public void ClearPlayerDataIndex()
    {
        CurrentlyJoinedPlayers = 0;
        foreach (var data in playerDatas)
        {
            data.HasJoined = false;
        }
    }

    public PlayerData GetPlayerData()
    {
        foreach (var playerData in playerDatas)
        {
            return playerData != null && !playerData.HasAssigned ? playerData : new PlayerData(1);
        }

        return new PlayerData(1);
    }
}
