using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    private Transform players;
    private Transform spawnPoint;
    private Transform[] respawnSpawnPoints;
    private LayerMask characterLayer;
    private readonly int mapHorizontalBorder = 80;
    private readonly float roundDelay = 2f;

    public string WinnerName { get; private set; }

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        characterLayer = LayerMask.GetMask("Character");
        players = transform.Find("Players");
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

    public void SpawnProjectile(Transform graphicParent, Vector2 spawnPoint)
    {
        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(3, 0), spawnPoint, Quaternion.identity);
        projectile.GetComponent<Kunai>().ProjectileInitialize((int)graphicParent.localScale.x);
    }
    public void SpawnProjectileSamurai(Transform graphicParent, Vector2 spawnPoint) {
        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 0), spawnPoint, Quaternion.identity);
        projectile.GetComponent<Shuriken>().ProjectileInitialize((int)graphicParent.localScale.x);
    }

    private void Start()
    {
        Time.timeScale = 1;
        Fabric.EventManager.Instance.PostEvent("Music");

        Invoke("StartRound", roundDelay);
    }

    private void StartRound()
    {
        PlayerDataManager.Instance.SpawnPlayers(players);
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

    public void Victory(string winnerName)
    {
        WinnerName = winnerName;
        UIManager.Instance.ChangePanelState(PANEL_STATE.VICTORY);
    }
}