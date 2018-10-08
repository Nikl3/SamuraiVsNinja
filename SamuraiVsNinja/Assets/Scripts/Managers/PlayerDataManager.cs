using UnityEngine;

public enum PLAYER_TYPE
{
    NINJA,
    SAMURAI
}

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public RuntimeAnimatorController[] RuntimeAnimatorControllers;
    public Sprite[] PlayerIconSprite;
    public Sprite[] DashIconSprite;
    public Sprite[] ProjectileIconSprite;

    public int TestPlayerAmount;

    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    #endregion VARIABLES

    #region PROPERTIES

    public PlayerData[] PlayerDatas
    {
        get;
        private set;
    }
    public int MaxPlayerNumber
    {
        get
        {
            return MAX_PLAYER_NUMBER;
        }
    }

    #endregion PROPERTIES

    private void Start()
    {
        CreatePlayerDatas();        
    }
   
    private Color SetPlayerColor(int playerID)
    {
        switch (playerID)
        {
            case 1:
                return Color.blue;

            case 2:
                return Color.red;

            case 3:
                return Color.green;

            case 4:
                return Color.yellow;

            default:
                return Color.magenta;
        }
    }
    private void CreatePlayerDatas()
    {
        PlayerDatas = new PlayerData[MAX_PLAYER_NUMBER];

        for (int i = 0; i < PlayerDatas.Length; i++)
        {
           
            PlayerDatas[i] = new PlayerData(i + 1, SetPlayerColor(i + 1))
            {
                
            };
        }
    }
    private void AddTestPlayers(int testPlayerAmount)
    {
        if (TestPlayerAmount > 0)
        {
            testPlayerAmount = testPlayerAmount > MAX_PLAYER_NUMBER ? MAX_PLAYER_NUMBER : testPlayerAmount;
            var randomTypeIndex = 0;
            for (int i = 0; i < testPlayerAmount; i++)
            {
                randomTypeIndex = Random.Range(0, 2);
                PlayerDatas[i].HasJoined = true;
                PlayerDatas[i].PlayerType =  (randomTypeIndex == 0 ? PLAYER_TYPE.NINJA : PLAYER_TYPE.SAMURAI);
            }
        }                   
    }

    public PlayerData GetPlayerData(int id)
    {
        return PlayerDatas[id];
    }
    public void ClearJoinedPlayers()
    {
        foreach (var playerData in PlayerDatas)
        {
            playerData.HasJoined = false;
        }
    }
    public void PlayerJoin(int playerID)
    {
        if (!PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = true;

            return;
        }
    }
    public void PlayerUnjoin(int playerID)
    {
        if (PlayerDatas[playerID - 1].HasJoined)
        {
            PlayerDatas[playerID - 1].HasJoined = false;          
            return;
        }
    } 
    public void SpawnPlayers()
    {
        AddTestPlayers(TestPlayerAmount);

        foreach (var playerData in PlayerDatas)
        {
            if (playerData.HasJoined)
            {                 
                playerData.Player = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0)).GetComponent<Player>();
                playerData.PlayerInfo = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 1)).GetComponent<PlayerInfo>();
                playerData.EndGameStats = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2)).GetComponent<EndGameStats>();
                playerData.Player.Initialize(playerData, playerData.PlayerType == PLAYER_TYPE.NINJA ? RuntimeAnimatorControllers[0] : RuntimeAnimatorControllers[1]);              
            }
        }
    }  
    public void UpdateEndGameStats()
    {
        foreach (var playerData in PlayerDatas)
        {
            if(playerData.HasJoined)
            {
                playerData.PlayerInfo.UpdateEndPanelStats();
            }
        }
    }
}
