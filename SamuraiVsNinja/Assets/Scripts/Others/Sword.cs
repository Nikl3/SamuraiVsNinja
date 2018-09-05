using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            print("player hit");
            var hittedPlayer = collision.GetComponent<Player>();
            hittedPlayer.PlayerInfo.TakeDMG();
        }
    }

}
