using UnityEngine;

public class PlayerDataManager : Singelton<PlayerDataManager>
{
    public RuntimeAnimatorController[] RuntimeAnimatorControllers;
    public Sprite[] PlayerIconSprite;
    public Sprite[] DashIconSprite;
    public Sprite[] ProjectileIconSprite;
    public Sprite[] EndgameSprite;

    [HideInInspector]
    public int TestPlayerAmount;

    #region VARIABLES

    private const int MAX_PLAYER_NUMBER = 4;

    #endregion VARIABLES

    #region PROPERTIES

    public PlayerData[] PlayerData
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
        PlayerData = new PlayerData[MAX_PLAYER_NUMBER];

        for (int i = 0; i < PlayerData.Length; i++)
        {
           
            PlayerData[i] = new PlayerData(i + 1, SetPlayerColor(i + 1))
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
                PlayerData[i].HasJoined = true;
                PlayerData[i].CharacterType =  (randomTypeIndex == 0 ? CHARACTER_TYPE.NINJA : CHARACTER_TYPE.SAMURAI);
            }
        }                   
    }

    public PlayerData GetPlayerData(int id)
    {
        return PlayerData[id];
    }
    public void ClearJoinedPlayers()
    {
        foreach (var playerData in PlayerData)
        {
            playerData.HasJoined = false;
        }
    }
    public void PlayerJoin(int playerID)
    {
        if (!PlayerData[playerID - 1].HasJoined)
        {
            PlayerData[playerID - 1].HasJoined = true;

            return;
        }
    }
    public void PlayerUnjoin(int playerID)
    {
        if (PlayerData[playerID - 1].HasJoined)
        {
            PlayerData[playerID - 1].HasJoined = false;          
            return;
        }
    } 
    public void SpawnPlayers()
    {
        AddTestPlayers(TestPlayerAmount);

        if(PlayerData == null || PlayerData.Length == 0)
        {
            Debug.LogError("No player data available!");
            return;
        }

        foreach (var data in PlayerData)
        {
            if (data.HasJoined)
            {                 
                data.Character = Instantiate(ResourceManager.Instance.GetPrefabByIndex(0, 0)).GetComponent<Character>();
                data.PlayerInfo = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 1)).GetComponent<PlayerInfo>();
                data.EndGameStats = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2)).GetComponent<EndGameStats>();
                data.Character.Initialize(data, data.CharacterType == CHARACTER_TYPE.NINJA ? RuntimeAnimatorControllers[0] : RuntimeAnimatorControllers[1]);              
            }
        }
    }  
    public void UpdateEndGameStats()
    {
        foreach (var playerData in PlayerData)
        {
            if(playerData.HasJoined)
            {
                playerData.PlayerInfo.UpdateEndPanelStats();
            }
        }
    }
}
