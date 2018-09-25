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
    public Sprite PlayerIconSprite { get; set; }
    public Sprite DashIconSprite { get; set; }
    public Sprite ProjectileIconSprite { get; set; }
    public Color PlayerColor { get; private set; }

    private readonly string playerName;

    public PlayerData(int id, Color playerColor = default(Color))
    {
        ID = id;
        playerName = "Player " + id;
        PlayerColor = playerColor;
    }
}
