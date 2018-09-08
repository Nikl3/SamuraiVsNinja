[System.Serializable]
public class PlayerData
{
    public int ID
    {
        get;
        private set;
    }
    public string PlayerName
    {
        get
        {
            return playerName;
        }
    }
    public bool HasJoined { get; set; }

    private readonly string playerName;

    public PlayerData(int id)
    {
        ID = id;           

        playerName = "Player " + id;
        //ActionButton = "Action" + "_J" + id;
        //HorizontalAxis = "Horizontal" + "_J" + id;
        //VerticalAxis = "Vertical" + "_J" + id;
        //JumpButton = "Jump" + "_J" + id;
        //MeleeAttackButton = "MeleeAttack" + "_J" + id;
        //RangeAttackButton = "RangeAttack" + "_J" + id;
        //DashButton = "Dash" + "_J" + id;
    }
}
