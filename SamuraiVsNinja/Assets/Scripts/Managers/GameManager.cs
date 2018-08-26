using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject localPlayerPrefab;
    private GameObject playerInfoPrefab;
    private Transform playerInfoContainer;

    private void Awake()
    {
        localPlayerPrefab = ResourceManager.Instance.GetPrefabByName("LocalPlayer");
        playerInfoPrefab = ResourceManager.Instance.GetPrefabByName("PlayerInfo");
        playerInfoContainer = transform.Find("HUD").transform.Find("PlayerInfoContainer");
    }

    private void Start()
    {
        // Invoke("Foo", 1f);
        Foo();
    }

    private void Foo()
    {
        for (int i = 1; i < InputManager.Instance.CurrentJoinedPlayers; i++)
        {
            SpawnObject(localPlayerPrefab, new Vector2(Random.Range(-20, 20), 10));

            var prefabInstance = SpawnObject(playerInfoPrefab, Vector2.zero, Quaternion.identity);
            prefabInstance.transform.SetParent(playerInfoContainer);
        }
    }

    private GameObject SpawnObject(GameObject prefab, Vector2 position = new Vector2(), Quaternion rotation = new Quaternion())
    {
        return Instantiate(prefab, position, rotation);      
    }
}
