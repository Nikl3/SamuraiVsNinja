using UnityEngine;

public class GameManager : Singelton<GameManager>
{
    public Transform[] RespawnSpawnPoints;
    private int[] usedSpawns;

    private void Start() {
        StartRound();
    }

    void StartRound() {
            PlayerDataManager.Instance.SpawnPlayers();
    }

    public Vector2 RandomSpawnPoint() {

        return Vector2.zero;
    }

    public void Victory(string winnerName) {
        PauseManager.Instance.VictoryPanel(winnerName);
    }

}