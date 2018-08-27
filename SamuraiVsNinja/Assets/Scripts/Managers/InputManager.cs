using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string ActionButton
    {
        get;
        private set;
    }
    public string HorizontalAxis
    {
        get;
        private set;
    }
    public string VerticalAxis
    {
        get;
        private set;
    }
    public string JumpButton
    {
        get;
        private set;
    }
    public string AttackButton
    {
        get;
        private set;
    }
    public string DashButton
    {
        get;
        private set;
    }

    public Color RandomColor
    {
        get
        {
            return randomColor;
        }
    }

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
    public bool HasJoined
    {
        get
        {
            return hasJoined;
        }
        set
        {
            hasJoined = value;
        }
    }

    private Color randomColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    private int id;
    private string playerName;
    private bool hasAssigned;
    private bool hasJoined;

    public PlayerData(int id)
    {
        this.id = id;
        playerName = "Player " + id;
        hasAssigned = true;

        ActionButton = "Action";
        HorizontalAxis = "Horizontal";
        VerticalAxis = "Vertical";
        JumpButton = "Jump";
        AttackButton = "Attack";
        DashButton = "Dash";

        SetControllerNumber(id);           
    }

    public void SetControllerNumber(int controllerNumber)
    {
        ActionButton = "Action" + "_J" + controllerNumber;
        HorizontalAxis = "Horizontal" + "_J" + controllerNumber;
        VerticalAxis = "Vertical" + "_J" + controllerNumber;
        JumpButton = "Jump" + "_J" + controllerNumber;
        AttackButton = "Attack" + "_J" + controllerNumber;
        DashButton = "Dash" + "_J" + controllerNumber;
    }
}

public class InputManager : SingeltonPersistant<InputManager>
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
