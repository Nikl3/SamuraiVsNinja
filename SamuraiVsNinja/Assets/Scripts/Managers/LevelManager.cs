using System.Collections;
using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    public Vector2[] PlayerSpawnPoints;
    public Vector2[] OnigiriSpawnPoints;
    public LayerMask CollisionLayer;

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
        //StartRound();
        //Invoke("StartRound", 1f);
        //Invoke("RunGame", 4f);
    }
    private void OnDrawGizmos()
    {
        if (PlayerSpawnPoints != null && OnigiriSpawnPoints != null)
        {
            // Draw player spawn points
            Gizmos.color = Color.white;
            var size = 1f;

            for (int i = 0; i < PlayerSpawnPoints.Length; i++)
            {
                Gizmos.DrawLine(PlayerSpawnPoints[i] - Vector2.up * size, PlayerSpawnPoints[i] + Vector2.up * size);
                Gizmos.DrawLine(PlayerSpawnPoints[i] - Vector2.left * size, PlayerSpawnPoints[i] + Vector2.left * size);
            }

            // Draw onigiri spawn points
            Gizmos.color = Color.yellow;

            for (int i = 0; i < OnigiriSpawnPoints.Length; i++)
            {
                Gizmos.DrawLine(OnigiriSpawnPoints[i] - Vector2.up * size, OnigiriSpawnPoints[i] + Vector2.up * size);
                Gizmos.DrawLine(OnigiriSpawnPoints[i] - Vector2.left * size, OnigiriSpawnPoints[i] + Vector2.left * size);
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
        PlayerDataManager.Instance.SpawnPlayers();
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
        var randomPositions = spawnPointType == 0 ? PlayerSpawnPoints : OnigiriSpawnPoints;
        Vector2 randomPosition = randomPositions[Random.Range(0, randomPositions.Length)];

        if(CanSpawnObjectAtPosition(randomPosition, Vector2.one, CollisionLayer))
        {
            return randomPosition;
        }

        return Vector2.zero;
    }
    public Vector2 GetSpawnPoint(int index)
    {
        return PlayerSpawnPoints[index];
    }
    public void EndGame(string winnerName)
    {
        GameIsRunning = false;

        // FIX ME!
        //EventManager.Instance.PostEvent("LevelTheme", EventAction.StopSound);
        Debug.LogError("Level Theme should end here!");
        //EventManager.Instance.PostEvent("Victory");
        Debug.LogError("Play Victory sound here!");

        WinnerName = winnerName;
        StartSpawnSushi();
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.VICTORY);
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
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(1, 0), randomPosition);         

            yield return null;
        }

        spawnOnigirisCoroutine = null;
    }
    private IEnumerator ISpawnSushi()
    {
        while (sushiDrop)
        {
            var randomDropdelay = Random.Range(0.5f, 1f);
            sushiDropTime -= Time.unscaledDeltaTime + randomDropdelay;
            if(sushiDropTime <= 0)
            {
                break;
            }
            ObjectPoolManager.Instance.SpawnObject(/*ResourceManager.Instance.GetPrefabByIndex(1, 1)*/
                ResourceManager.Instance.GetPrefabByIndex(5, 7),
                CameraEngine.Instance.ScreenToWorldPoint(new Vector2(
                    Random.Range(0, Screen.width), 
                    Random.Range(0, Screen.height)
                    )));
            yield return new WaitForSecondsRealtime(randomDropdelay);
        }

        sushiDropTime = 10f;
        sushiDrop = false;
        spawnSushiCoroutine = null;
    }
}