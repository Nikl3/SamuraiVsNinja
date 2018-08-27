using UnityEngine;

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

    private void SetControllerNumber(int controllerNumber)
    {
        ActionButton = "Action" + "_J" + controllerNumber;
        HorizontalAxis = "Horizontal" + "_J" + controllerNumber;
        VerticalAxis = "Vertical" + "_J" + controllerNumber;
        JumpButton = "Jump" + "_J" + controllerNumber;
        AttackButton = "Attack" + "_J" + controllerNumber;
        DashButton = "Dash" + "_J" + controllerNumber;
    }
}
