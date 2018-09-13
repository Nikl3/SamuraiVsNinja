using UnityEngine;

public class PlayerTriggerConroller : MonoBehaviour {
    private Player player;
    private Vector2 knockbackForce = new Vector2(10, 20);

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (player.CurrentState == PlayerState.NORMAL) {
            if (collision.CompareTag("Onigiri")) {
                player.AddOnigiri(1);
                Destroy(collision.gameObject);
                return;
            }
            if (collision.CompareTag("Spike")) {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                player.TakeDamage(hitDirection, knockbackForce, 1);
                return;
            }

            if (collision.CompareTag("Player") && player.PlayerEngine.IsDashing) {
                print("hit");
                var hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                player.TakeDamage(hitDirection, new Vector2(40, 15), 0);
                return;

            }
        }
    }
}
