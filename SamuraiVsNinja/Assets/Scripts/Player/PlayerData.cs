using UnityEngine;

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
    public Color PlayerColor { get; private set; }

    private readonly string playerName;

    public PlayerData(int id, Color playerColor = default(Color))
    {
        ID = id;
       
        PlayerColor = playerColor;

        DebugManager.Instance.DebugMessage(3,playerColor.ToString());

        playerName = "Player " + id;
    }
}
