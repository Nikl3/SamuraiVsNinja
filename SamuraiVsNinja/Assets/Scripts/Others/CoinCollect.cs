using UnityEngine;
public class CoinCollect : MonoBehaviour
{
	private PlayerInput playerInput;

	private void Awake()
    {
		playerInput = GetComponent<PlayerInput>();		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Coin")
		{
			//pickup coin
			Destroy(collision.gameObject);
            //add coin to player coincount
            playerInput.PlayerInfo.ModifyCoinValues(1);
		}
	}
}
