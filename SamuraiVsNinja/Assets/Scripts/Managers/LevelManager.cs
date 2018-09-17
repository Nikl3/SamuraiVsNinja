using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    private Transform spawnPoint;
    private Transform[] respawnSpawnPoints;
    private LayerMask characterLayer;

    public int[] UsedSpawns
    {
        get;
        set;
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
        StartRound();

    }

    private void StartRound()
    {
        PlayerDataManager.Instance.SpawnPlayers();
        Fabric.EventManager.Instance.PostEvent("Music");
    }

    public Vector2 RandomSpawnPoint()
    {
        int randomPosIndex = Random.Range(0, respawnSpawnPoints.Length);
        Vector2 randomPos = respawnSpawnPoints[randomPosIndex].position;
        if (!Physics2D.OverlapBox(randomPos, Vector2.one, 0f, characterLayer))
        {
            return randomPos;
        }
        return RandomSpawnPoint();
    }

    public void Victory(string winnerName)
    {
        GameUIManager.Instance.VictoryPanel(winnerName);
    }
}