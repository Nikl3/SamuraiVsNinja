using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject playerPrefab;

    private void Awake()
    {
        playerPrefab = ResourceManager.Instance.GetPrefabByName("Player");
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < PlayerDataManager.Instance.CurrentlyJoinedPlayers; i++)
        {
            SpawnObject(playerPrefab, new Vector2(Random.Range(-20, 20), 10));
        }
    }

    private GameObject SpawnObject(GameObject prefab, Vector2 position = new Vector2(), Quaternion rotation = new Quaternion())
    {
        return Instantiate(prefab, position, rotation);      
    }
}
