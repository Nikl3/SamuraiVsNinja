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
    public Player Player
    {
        get;
        private set;
    }
    public PlayerInfo PlayerInfo
    {
        get;
        private set;
    }
    public EndGameStats EndGameStats
    {
        get;
        private set;
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

    public PlayerData(int id, Player player, PlayerInfo playerInfo, EndGameStats endGameStats, Color playerColor)
    {
        ID = id;
        Player = player;
        PlayerInfo = playerInfo;
        EndGameStats = endGameStats;
        playerName = "Player " + id;
        PlayerColor = playerColor;

        PlayerInfo.Owner = Player;

        Player.gameObject.SetActive(false);
        PlayerInfo.gameObject.SetActive(false);
        EndGameStats.gameObject.SetActive(false);
    }
}
