using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Transform[] RespawnSpawnPoints;

    private void Start()
    {
        PlayerDataManager.Instance.SpawnPlayers();
    }
}
