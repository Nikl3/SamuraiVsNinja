using UnityEngine;

public class PlayerDataManager : SingeltonPersistant<PlayerDataManager>
{
    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    private PlayerData[] playersData = new PlayerData[MAX_PLAYER_NUMBER];

    private bool canJoin = false;

    private int playerDataIndex;
    private int usedPlayerDataIndex;

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
    public int CurrentJoinedPlayers
    {
        get
        {
            return playerDataIndex + 1;
        }
    }

    #endregion PROPERTIES

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < playersData.Length; i++)
        {
            playersData[i] = new PlayerData(i + 1);
        }
    }

    private void Update()
    {
        if (!canJoin)
            return;

        // Muuta unityn inputeista oikeat input manageriin vastaavat stringit 
        if (InputManager.Instance.GetButtonDown(0, InputAction.Action))
        {
            print("Player 0" + " pressed: Action " + InputAction.Action);
        }

        /*
        if (Input.GetButtonDown("Action_J1"))
        {
            print("ACTION_J1");

            foreach (var data in playersData)
            {
                if(data.ID == 1)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;
                        PlayerJoin();
                    }
                }
            }
        }

        if (Input.GetButtonDown("Action_J2"))
        {
            print("ACTION_J2");
            foreach (var data in playersData)
            {
                if (data.ID == 2)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;
                        PlayerJoin();
                    }
                }
            }
        }

        if (Input.GetButtonDown("Action_J3"))
        {
            print("ACTION_J3");
            foreach (var data in playersData)
            {
                if (data.ID == 3)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;
                        PlayerJoin();
                    }
                }
            }
        }

        if (Input.GetButtonDown("Action_J4"))
        {
            print("ACTION_J4");
            foreach (var data in playersData)
            {
                if (data.ID == 4)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;
                        PlayerJoin();
                    }
                }
            }
        }

        if (Input.GetButtonDown("Cancel_J1"))
        {
            print("Cancel_J1");

            foreach (var data in playersData)
            {
                if (data.ID == 1)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;

                    }
                }
            }
        }

        if (Input.GetButtonDown("Cancel_J2"))
        {
            print("Cancel_J2");
            foreach (var data in playersData)
            {
                if (data.ID == 2)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;

                    }
                }
            }
        }

        if (Input.GetButtonDown("Cancel_J3"))
        {
            print("Cancel_J3");
            foreach (var data in playersData)
            {
                if (data.ID == 3)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = true;
                    }
                }
            }
        }

        if (Input.GetButtonDown("Cancel_J4"))
        {
            print("Cancel_J4");
            foreach (var data in playersData)
            {
                if (data.ID == 4)
                {
                    if (!data.HasJoined)
                    {
                        data.HasJoined = false;
                        
                    }
                }
            }
        }
        */
    }

    private void PlayerJoin()
    {
        if (playerDataIndex >= MAX_PLAYER_NUMBER)
        {
            playerDataIndex = MAX_PLAYER_NUMBER;
            return;
        }

        MainMenuManager.Instance.SetJoinField(playerDataIndex, playersData[playerDataIndex].PlayerName);
        playerDataIndex++;      
    }

    public PlayerData GetPlayerData()
    {
        var playerData = playersData[usedPlayerDataIndex];
        usedPlayerDataIndex++;
        return playerData;
    }

    public void ClearPlayersData()
    {
        playersData = new PlayerData[MAX_PLAYER_NUMBER];
        playerDataIndex = 0;
    }
}
