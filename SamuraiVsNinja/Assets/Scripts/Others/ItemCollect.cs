using UnityEngine;
public class ItemCollect : MonoBehaviour
{
	private Player player;

	private void Awake()
	{
		player = GetComponent<Player>();		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Onigiri")
		{
			//pickup coin
			Destroy(collision.gameObject);
			//add coin to player coincount
			player.PlayerInfo.ModifyCoinValues(1);
		}
	}
}
