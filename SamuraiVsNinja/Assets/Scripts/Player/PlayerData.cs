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
    public string MeleeAttackButton
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

    [SerializeField] private int id;
    [SerializeField] private string playerName;
    [SerializeField] private bool hasJoined;

    public PlayerData(int id)
    {
        this.id = id;
        playerName = "Player " + id;
        ActionButton = "Action" + "_J" + id;
        HorizontalAxis = "Horizontal" + "_J" + id;
        VerticalAxis = "Vertical" + "_J" + id;
        JumpButton = "Jump" + "_J" + id;
        MeleeAttackButton = "MeleeAttack" + "_J" + id;
        RangeAttackButton = "RangeAttack" + "_J" + id;
        DashButton = "Dash" + "_J" + id;
    }
}
