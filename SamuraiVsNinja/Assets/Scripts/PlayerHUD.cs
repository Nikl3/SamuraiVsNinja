using UnityEngine;

public class PlayerHUD : MonoBehaviour {
    public PlayerPanel[] playerPanels;

    public void SetPlayerActive(int playerIndex, Color color) {
        playerPanels[playerIndex].SetPlayer(color);
    }
}
