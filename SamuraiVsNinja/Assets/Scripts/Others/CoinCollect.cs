using UnityEngine;
public class CoinCollect : MonoBehaviour
{
	//private GameManager gm;
	//private PlayerInput pl;

	private void Start() {
		//gm = FindObjectOfType<GameManager>();
		//pl = GetComponent<PlayerInput>();		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin")
		{
			//pickup coin
			Destroy(collision.gameObject);
			//add coin to player coincount
			//gm.coinsText = GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
			//pl.ps.Coins++;
			//pl.ps.PlayerInfo.GetComponentInChildren<TextMeshProUGUI>().text = "" + pl.ps.Coins;
		}
	}
}
