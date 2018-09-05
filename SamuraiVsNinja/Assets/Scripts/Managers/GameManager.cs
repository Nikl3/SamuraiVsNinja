using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        PlayerDataManager.Instance.SpawnPlayers();
    }
}
