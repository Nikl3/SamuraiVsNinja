using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public CHARACTER_TYPE CharacterType
    {
        get;
        set;
    }
    public Character Character
    {
        get;
        set;
    }
    public PlayerInfo PlayerInfo
    {
        get;
        set;
    }
    public EndGameStats EndGameStats
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
        get;
    }
    public bool HasJoined
    {
        get;
        set;
    }
    public Color PlayerColor
    {
        get;
        private set;
    }

    public PlayerData(int id,  Color playerColor)
    {
        ID = id;
        PlayerName = "Player " + id;
        PlayerColor = playerColor;
    }
}
