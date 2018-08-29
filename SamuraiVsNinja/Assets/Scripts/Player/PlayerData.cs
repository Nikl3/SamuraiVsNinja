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
    public string RangeAttackButton
    {
        get;
        private set;
    }
    public string DashButton
    {
        get;
        private set;
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
    public bool HasAssigned
    {
        get
        {
            return hasAssigned;
        }
        set
        {
            hasAssigned = value;
        }
    }

    [SerializeField] private int id;
    [SerializeField] private string playerName;
    [SerializeField] private bool hasJoined;
    [SerializeField] private bool hasAssigned;

    public PlayerData(int id)
    {
        this.id = id;
        playerName = "Player " + id;
        ActionButton = "Action";
        HorizontalAxis = "Horizontal";
        VerticalAxis = "Vertical";
        JumpButton = "Jump";
        AttackButton = "Attack";
        DashButton = "Dash";
        RangeAttackButton = "RangeAttack";

        SetControllerNumber(id);
    }

    private void SetControllerNumber(int controllerNumber)
    {
        ActionButton = "Action" + "_J" + controllerNumber;     
        HorizontalAxis = "Horizontal" + "_J" + controllerNumber;      
        VerticalAxis = "Vertical" + "_J" + controllerNumber; 
        JumpButton = "Jump" + "_J" + controllerNumber;   
        AttackButton = "Attack" + "_J" + controllerNumber;
        DashButton = "Dash" + "_J" + controllerNumber;
        RangeAttackButton = "RangeAttack" + "_J" + controllerNumber;
    }
}
