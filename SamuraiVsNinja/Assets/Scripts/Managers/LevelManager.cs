using System.Collections;
using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    private bool gameIsRunning;

    [SerializeField]
    private Vector2[] playerSpawnPoints;
    [SerializeField]
    private Vector2[] onigiriSpawnPoints;

    private Coroutine spawnOnigirisCoroutine;

    [SerializeField]
    private LayerMask collisionLayer;
    private readonly int mapHorizontalBorder = 80;

    public string WinnerName
    {
        get;
        private set;
    }

    private void Start()
    {
        StartRound();
    }

    private void StartRound()
    {
        gameIsRunning = true;

        Time.timeScale = 1;
        Fabric.EventManager.Instance.PostEvent("Music");
        PlayerDataManager.Instance.SpawnPlayers();

        //StartSpawnItem(ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(1, 0)));
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
    public void Victory(string winnerName)
    {
        gameIsRunning = false;

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
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 1), transform.position);
        }
        else if (objectToTeleport.position.x < -mapHorizontalBorder)
        {
            objectToTeleport.position = new Vector2(-objectToTeleport.position.x, objectToTeleport.position.y);
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 1), transform.position);
        }        
    }
    public void SpawnProjectile(Player player, Transform graphicParent, Vector2 spawnPoint, int projectileTypeIndex)
    {
        var projectile = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(3, projectileTypeIndex == 0 ? 0 : 1), spawnPoint, Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInitialize(player, (int)graphicParent.localScale.x);
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

    private void StartSpawnItem(GameObject prefab)
    {
        if(spawnOnigirisCoroutine == null)
        {
            spawnOnigirisCoroutine = StartCoroutine(ISpawnItem(prefab));
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

    private IEnumerator ISpawnItem(GameObject prefab)
    {
        while (gameIsRunning)
        {
            yield return new WaitForSeconds(2f);

            var randomPosition = RandomSpawnPosition(1);

            if(randomPosition != Vector2.zero)
            {
                print(randomPosition);
                ObjectPoolManager.Instance.SpawnObject(prefab, randomPosition);
            }

            yield return null;
        }

        spawnOnigirisCoroutine = null;
    }
}