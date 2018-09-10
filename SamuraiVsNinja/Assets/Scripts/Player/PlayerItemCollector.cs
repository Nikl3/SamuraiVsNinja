using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.CurrentState == PlayerState.NORMAL)
        {
            if (collision.CompareTag("Onigiri"))
            {
                player.AddOnigiri(1);
                Destroy(collision.gameObject);
                return;
            }
            if (collision.CompareTag("Spike"))
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                player.TakeDamage(hitDirection);
                return;
            }
        }
    }
}
