using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour {

    GameManager gm;

    private void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin")
		{
			//pickup coin
			print("coin collected!");
			Destroy(collision.gameObject);
            //add coin to player coincount
            gm.P1CoinCount++;
            gm.coinsText.text = "" + gm.P1CoinCount;
		}
	}
}
