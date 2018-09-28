using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    private Transform spawnPoint;
    private Transform[] respawnSpawnPoints;
    private LayerMask characterLayer;
    private readonly int mapHorizontalBorder = 80;

    public string WinnerName
    {
        get;
        private set;
    }

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        characterLayer = LayerMask.GetMask("Character");
        GetSpawnPoints();
    }
    private void GetSpawnPoints()
    {
        spawnPoint = transform.GetChild(0);
        var childCount = spawnPoint.childCount;
        respawnSpawnPoints = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            respawnSpawnPoints[i] = spawnPoint.GetChild(i);
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        Fabric.EventManager.Instance.PostEvent("Music");

        StartRound();
    }

    private void StartRound()
    {
        PlayerDataManager.Instance.SpawnPlayers();
    }

    public Vector2 RandomSpawnPoint()
    {
        int randomPosIndex = Random.Range(0, respawnSpawnPoints.Length);
        Vector2 randomPosition = respawnSpawnPoints[randomPosIndex].position;
        if (!Physics2D.OverlapBox(randomPosition, Vector2.one, 0f, characterLayer))
        {
            return randomPosition;
        }
        return RandomSpawnPoint();
    }
    public Vector2 GetSpawnPoint(int index)
    {
        return respawnSpawnPoints[index].position;
    }
    public void Victory(string winnerName)
    {
        WinnerName = winnerName;
        UIManager.Instance.ChangePanelState(PANEL_STATE.VICTORY);
        foreach (var player in PlayerDataManager.Instance.CurrentlyJoinedPlayers)
        {
            player.PlayerInfo.UpdateEndPanelStats();
        }
    }
    public void TeleportObject(Transform objectToTeleport)
    {
        if (objectToTeleport.position.x > mapHorizontalBorder)
        {
            objectToTeleport.position = new Vector2(-objectToTeleport.position.x, objectToTeleport.position.y);
            Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 1), objectToTeleport.position, Quaternion.identity);
        }
        else if (objectToTeleport.position.x < -mapHorizontalBorder)
        {
            objectToTeleport.position = new Vector2(-objectToTeleport.position.x, objectToTeleport.position.y);
            Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 1), objectToTeleport.position, Quaternion.identity);
        }
    }

    public void SpawnProjectile(Player player, Transform graphicParent, Vector2 spawnPoint, int projectileTypeIndex)
    {
        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(3, projectileTypeIndex == 0 ? 0 : 1), spawnPoint, Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInitialize(player, (int)graphicParent.localScale.x);
    }
}