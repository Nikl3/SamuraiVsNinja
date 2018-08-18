using UnityEngine;

public class CoinCollect : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin")
		{
			//pickup coin
			print("coin collected!");
			Destroy(collision.gameObject);
		}
	}
}
