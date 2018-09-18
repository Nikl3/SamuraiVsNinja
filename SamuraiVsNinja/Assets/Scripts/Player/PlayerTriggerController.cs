using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private Player player;
    private Vector2 knockbackForce = new Vector2(10, 20);

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.CurrentState == PlayerState.NORMAL)
        {
            var hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            switch (collision.tag)
            {
                case "Onigiri":
                    Fabric.EventManager.Instance.PostEvent("Pickup");
                    player.AddOnigiri(1);
                    Destroy(collision.gameObject);
                    break;
                case "Pickup":

                    player.TakeDamage(hitDirection, knockbackForce, 1);
                    break;
                case "Spike":

                    player.TakeDamage(hitDirection, knockbackForce, 1);
                    break;
                case "Player":

                    var hittedPlayer = collision.gameObject.GetComponentInParent<Player>();
                    hittedPlayer.TakeDamage(hitDirection, new Vector2(-40, 15), 0);
                    break;
                case "Killzone":

                    player.TakeDamage(hitDirection, Vector2.zero, 3);
                    break;
            }
        }    
    }
}
