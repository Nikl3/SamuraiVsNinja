using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour {

    GameObject[] player;



	void Start () {
        player = GameObject.FindGameObjectsWithTag("Player");
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Coin") {
            //pickup coin
            print("coin collected!");
            Destroy(collision.gameObject);
        }
    }

}
