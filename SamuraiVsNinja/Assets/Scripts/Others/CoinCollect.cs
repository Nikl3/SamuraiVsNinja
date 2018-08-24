using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinCollect : MonoBehaviour {

	GameManager gm;
    Player pl;

	private void Start() {
		gm = FindObjectOfType<GameManager>();
		pl = GetComponent<Player>();
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin")
		{
			//pickup coin
			Destroy(collision.gameObject);
            //add coin to player coincount
            gm.coinsText = GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
            pl.ps.Coins++;
			pl.ps.PlayerInfo.GetComponentInChildren<TextMeshProUGUI>().text = "" + pl.ps.Coins;
		}
	}
}
