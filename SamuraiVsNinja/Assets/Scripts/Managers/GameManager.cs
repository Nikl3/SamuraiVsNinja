using UnityEngine;

public class GameManager : Singelton<GameManager>
{
    public Transform[] RespawnSpawnPoints;

    private void Start() {
        StartRound();
    }

    void StartRound() {

        PlayerDataManager.Instance.SpawnPlayers();

    }

    public void Victory(string winnerName) {
        PauseManager.Instance.VictoryPanel(winnerName);
    }

}