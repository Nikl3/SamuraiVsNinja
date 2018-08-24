using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Player playerPrefab;
    public Color[] playerColours;
    public PlayerHUD playerHud;

    int maxPlayersCount = 4;
    int joinedPlayersCount = 0;
    bool[] playerInputAlreadyInUse;
    string[] actionButtons;

    private void Awake(){
        //count keyboard in (index 0)
        int maxInputCount = maxPlayersCount + 1;

        playerInputAlreadyInUse = new bool[maxInputCount];

        //create action button strings (optimization)
        actionButtons = new string[maxInputCount];

        //keyboard button
        actionButtons[0] = "Action1";

        //external controllers
        for (int i = 1; i <= maxPlayersCount; i++)
        {
            actionButtons[i] = "Action1_J" + (i);
        }
    }

    void Update () {
        //players joining input
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (Input.GetButtonDown(actionButtons[i]))
            {
                PlayerJoin(i);
            }
        }
    }

    private void PlayerJoin(int inputIndex){
        if (playerInputAlreadyInUse[inputIndex]) return;//player joined already
        if (joinedPlayersCount == maxPlayersCount) return; //all players joined already
        playerInputAlreadyInUse[inputIndex] = true;

        //player stats
        int playerIndex = joinedPlayersCount;
        Color playerColor = playerColours[playerIndex];

        //setup hud
        playerHud.SetPlayerActive(playerIndex, playerColor);

        //create player prefab
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.Init(playerColor, inputIndex);

        joinedPlayersCount++;
    }
}
