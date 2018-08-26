using UnityEngine;

public enum InputActions
{
    None = 0,
    MoveHorizontal = 1,
    Jump = 2,
    MeleeAttack = 3,
    ThrowAttack = 4,
    Dash = 5,
    Cancel = 6
}

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

    private Color randomColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    private int id;
    private string playerName;
    private bool hasAssigned;

    public PlayerData(int id)
    {
        this.id = id;
        playerName = "Player " + id;
        hasAssigned = true;

        if(id == 0)
        {
            ActionButton = "Action";
            HorizontalAxis = "Horizontal";
            VerticalAxis = "Vertical";
            JumpButton = "Jump";
            AttackButton = "Attack";
            DashButton = "Dash";
        }
        else
        {
            SetControllerNumber(id);
        }     
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

    private PlayerData[] playersData;

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
        ClearPlayersData();
    }

    private void Update()
    {
        if (!canJoin)
            return;

        if (Input.GetButtonDown("Action") || Input.GetButtonDown("Action_J1"))
        {
            AddNewPlayerData();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            // RemovePlayerData();
        }
    }

    private void AddNewPlayerData()
    {
        if (playerDataIndex >= MAX_PLAYER_NUMBER)
        {
            playerDataIndex = MAX_PLAYER_NUMBER;
            return;
        }

        playersData[playerDataIndex] = CreateNewPlayerData();

        MainMenuManager.Instance.SetJoinField(playerDataIndex, playersData[playerDataIndex].PlayerName);
        playerDataIndex++;      
    }

    private PlayerData CreateNewPlayerData()
    {
        foreach (var playerData in playersData)
        {
            if(!playerData.HasAssigned)
            {
                return new PlayerData(playerDataIndex + 1);
            }       
        }

        Debug.LogError("Creating dummy player data!");
        return new PlayerData(playerDataIndex);
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
