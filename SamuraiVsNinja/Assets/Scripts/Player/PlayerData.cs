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
    public Color PlayerColor { get; set; }

    private readonly string playerName;

    public PlayerData(int id)
    {
        ID = id;
        PlayerColor = new Color(1,1,1,0.4f);

        playerName = "Player " + id;
    }
}
