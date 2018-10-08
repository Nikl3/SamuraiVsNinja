using System.Collections;
using UnityEngine;
using Fabric;

public class LevelManager : Singelton<LevelManager>
{
    [SerializeField]
    private Vector2[] playerSpawnPoints;
    [SerializeField]
    private Vector2[] onigiriSpawnPoints;
    [SerializeField]
    private LayerMask collisionLayer;
    private GameObject onigiriPrefab, sushiPrefab;
    private bool sushiDrop = false;
    private readonly int mapHorizontalBorder = 80;
    private float sushiDropTime = 10;
    private Coroutine spawnOnigirisCoroutine;
    private Coroutine spawnSushiCoroutine;

    public string WinnerName
    {
        get;
        private set;
    }

    public bool GameIsRunning
    {
        get;
        private set;
    }

    private void RunGame()
    {
        GameIsRunning = true;
        StartSpawnOnigiris();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        onigiriPrefab = ResourceManager.Instance.GetPrefabByIndex(1, 0);
        sushiPrefab = ResourceManager.Instance.GetPrefabByIndex(1, 1);
        Invoke("StartRound",2);
        Invoke("RunGame", 4f);
    }
    private void OnDrawGizmos()
    {
        if (playerSpawnPoints != null && onigiriSpawnPoints != null)
        {
            // Draw player spawn points
            Gizmos.color = Color.white;
            float size = 1f;

            for (int i = 0; i < playerSpawnPoints.Length; i++)
            {
                Gizmos.DrawLine(playerSpawnPoints[i] - Vector2.up * size, playerSpawnPoints[i] + Vector2.up * size);
                Gizmos.DrawLine(playerSpawnPoints[i] - Vector2.left * size, playerSpawnPoints[i] + Vector2.left * size);
            }

            // Draw onigiri spawn points
            Gizmos.color = Color.yellow;

            for (int i = 0; i < onigiriSpawnPoints.Length; i++)
            {
                Gizmos.DrawLine(onigiriSpawnPoints[i] - Vector2.up * size, onigiriSpawnPoints[i] + Vector2.up * size);
                Gizmos.DrawLine(onigiriSpawnPoints[i] - Vector2.left * size, onigiriSpawnPoints[i] + Vector2.left * size);
            }
        }
    }

    private bool CanSpawnObjectAtPosition(Vector2 position, Vector2 checkArea, LayerMask collisionLayer, float angle = 0f)
    {
        if (!Physics2D.OverlapBox(position, checkArea, angle, collisionLayer))
        {
            return true;
        }

        return false;
    }
 
    private void StartRound()
    {
        Time.timeScale = 1;
        PlayerDataManager.Instance.SpawnPlayers();
        //StartSpawnSushi();
    }
    private void StartSpawnOnigiris()
    {
        if (spawnOnigirisCoroutine == null)
        {
            spawnOnigirisCoroutine = StartCoroutine(ISpawnOnigiris());
        }
    }
    private void StartSpawnSushi()
    {
        if (spawnSushiCoroutine == null)
        {
            sushiDrop = true;
            spawnSushiCoroutine = StartCoroutine(ISpawnSushi());
        }
    }

    /// <summary>
    /// spawnPointTypes 0 = playerSpawnPoints 1 = onigiriSpawnPoints
    /// </summary>
    /// <param name="spawnPointType"></param>
    /// <returns></returns>
    public Vector2 RandomSpawnPosition(int spawnPointType)
    {
        var randomPositions = spawnPointType == 0 ? playerSpawnPoints : onigiriSpawnPoints;
        Vector2 randomPosition = randomPositions[Random.Range(0, randomPositions.Length)];

        if(CanSpawnObjectAtPosition(randomPosition, Vector2.one, collisionLayer))
        {
            return randomPosition;
        }

        return Vector2.zero;
    }
    public Vector2 GetSpawnPoint(int index)
    {
        return playerSpawnPoints[index];
    }
    public void EndGame(string winnerName)
    {
        GameIsRunning = false;
        EventManager.Instance.PostEvent("LevelTheme", EventAction.StopSound);
        EventManager.Instance.PostEvent("Victory");
        WinnerName = winnerName;    

        UIManager.Instance.ChangePanelState(PANEL_STATE.VICTORY);
    }
    public void TeleportObject(Transform objectToTeleport)
    {
        if (objectToTeleport.position.x > mapHorizontalBorder)
        {
            objectToTeleport.position = new Vector2(-objectToTeleport.position.x, objectToTeleport.position.y);
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 1), objectToTeleport.position);
        }
        else if (objectToTeleport.position.x < -mapHorizontalBorder)
        {
            objectToTeleport.position = new Vector2(-objectToTeleport.position.x, objectToTeleport.position.y);
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 1), objectToTeleport.position);
        }        
    }

    private IEnumerator ISpawnOnigiris()
    {
        while (GameIsRunning)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            var randomPosition = RandomSpawnPosition(1);
            
            if(randomPosition != Vector2.zero)
            ObjectPoolManager.Instance.SpawnObject(onigiriPrefab, randomPosition);         

            yield return null;
        }

        spawnOnigirisCoroutine = null;
    }
    private IEnumerator ISpawnSushi()
    {
        while (sushiDrop)
        {
            sushiDropTime -= Time.deltaTime;

            if(sushiDropTime <= 0)
            {
                break;
            }   

            ObjectPoolManager.Instance.SpawnObject(sushiPrefab, new Vector2(Random.Range(-60, 70), 60));

            yield return null;
        }

        sushiDrop = false;
        spawnSushiCoroutine = null;
    }
}