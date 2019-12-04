using UnityEngine;

public class Sword : MonoBehaviour
{
	private Character player;
	private Vector2 knockbackForce = new Vector2(40, 10);

	private void Awake ()
	{
		player = GetComponentInParent<Character>();		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (player.CurrentState == CHARACTER_STATE.NORMAL)
		{
			if (collision.CompareTag("Player"))
			{
				var hittedPlayer = collision.GetComponentInParent<Character>();

				if (hittedPlayer != null)
				{
					var hitDirection = collision.transform.position - transform.position;
					hitDirection.x = -hitDirection.x;
					hitDirection = hitDirection.normalized;
					hittedPlayer.TakeDamage(player, hitDirection, knockbackForce, 1, 1);

					ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 2), collision.transform.position);
				}
			}
		}
	}
}
