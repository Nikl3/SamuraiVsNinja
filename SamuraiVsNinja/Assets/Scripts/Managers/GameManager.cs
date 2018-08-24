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
        if(InputManager.Instance.CurrentlyJoinedPlayers > 0)
        {
            Debug.LogWarning("No joined players or Player 1 is keyboard.");
        }

        SpawnObject(localPlayerPrefab);
        SpawnObject(playerInfoPrefab, Vector2.zero, Quaternion.identity, playerInfoContainer);
    }

    private GameObject SpawnObject(GameObject prefab, Vector2 position = new Vector2(), Quaternion rotation = new Quaternion(), Transform parent = null)
    {
        if (parent == null)
            parent = transform;

        var index = 1;
        var prefabInstance = Instantiate(prefab, position, rotation);
        prefabInstance.name = prefab.name + " " + index;
        prefabInstance.transform.SetParent(parent);
        return prefabInstance;
    }
}
