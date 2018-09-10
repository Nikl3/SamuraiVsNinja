using UnityEngine;

public class Sword : MonoBehaviour
{
	Player player;

	private void Awake ()
	{
		player = GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (player.CurrentState == PlayerState.Normal)
		{
			if (collision.CompareTag("Player"))
			{
				//print("player hit");
				var hittedPlayer = collision.GetComponent<Player>();

                Vector2 direction = collision.transform.position - transform.position;          
                direction = direction.normalized;
                Debug.Log(direction);

                hittedPlayer.PlayerInfo.TakeDamage(hittedPlayer, direction);
			}
		}
	}
}
