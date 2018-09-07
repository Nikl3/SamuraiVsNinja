using UnityEngine;

public class GameManager : Singelton<GameManager>
{
    public Transform[] RespawnSpawnPoints;
    private int[] usedSpawns;
    [SerializeField]
    LayerMask playerLayer;

    private void Start() {
        StartRound();
    }

    void StartRound() {
            PlayerDataManager.Instance.SpawnPlayers();
    }

    public Vector2 RandomSpawnPoint() {
        int randomPosIndex = Random.Range(0, RespawnSpawnPoints.Length);
        Vector2 randomPos = RespawnSpawnPoints[randomPosIndex].position;
        if (!Physics2D.OverlapBox(randomPos, Vector2.one, 0f, playerLayer)) {
            return randomPos;
        }
        return RandomSpawnPoint();
    }

    public void Victory(string winnerName) {
        PauseManager.Instance.VictoryPanel(winnerName);
    }

}
