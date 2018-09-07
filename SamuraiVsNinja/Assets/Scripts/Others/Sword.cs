using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    Player player;

	void Awake () {
        player = GetComponent<Player>();
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (player.CurrentState == PlayerState.Normal) {
            if (collision.CompareTag("Player")) {
                print("player hit");
                var hittedPlayer = collision.GetComponent<Player>();
                hittedPlayer.PlayerInfo.TakeDamage(hittedPlayer);
            }
        }
    }

}
