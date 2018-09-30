using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private readonly string playerName;

    public PLAYER_TYPE PlayerType
    {
        get;
        set;
    }
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
    public Color PlayerColor { get; private set; }

    public PlayerData(int id, Color playerColor = default(Color))
    {
        ID = id;
        playerName = "Player " + id;
        PlayerColor = playerColor;
    }
}
