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

		if (player.CurrentState == PlayerState.NORMAL)
		{
			if (collision.CompareTag("Player"))
			{
				var hittedPlayer = collision.GetComponent<Player>();

				if (hittedPlayer != null)
				{
					var hitDirection = collision.transform.position - transform.position;
					hitDirection = hitDirection.normalized;
					hittedPlayer.TakeDamage(hitDirection);
				}
			}
		}
	}
}
