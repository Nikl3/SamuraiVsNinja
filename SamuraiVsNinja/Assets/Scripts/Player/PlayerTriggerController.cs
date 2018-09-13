using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private Player player;
    private Vector2 knockbackForce = new Vector2(10, 20);

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
                Fabric.EventManager.Instance.PostEvent("Pickup");
                player.AddOnigiri(1);
                Destroy(collision.gameObject);
                return;
            }

            if (collision.CompareTag("Spike"))
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                player.TakeDamage(hitDirection, knockbackForce, 1);
                return;
            }

            if (collision.CompareTag("Player") && player.AnimatorController.GetAnimaionState("IsDashing"))
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                var hittedPlayer = collision.gameObject.GetComponent<Player>();
                hittedPlayer.TakeDamage(hitDirection, new Vector2(-40, 15), 0);
                return;
            }

            if (collision.CompareTag("Killzone"))
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                player.TakeDamage(hitDirection,Vector2.zero, 3);
            }
        }
    }
}
