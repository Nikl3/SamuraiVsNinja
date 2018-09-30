using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private Player player;
    private Vector2 knockbackForce = new Vector2(10, 20);

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
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

                    ObjectPoolManager.Instance.DespawnObject(collision.gameObject);
                    break;

                case "Sushi":
                    Fabric.EventManager.Instance.PostEvent("Pickup");

                    print("+1 Health");

                    ObjectPoolManager.Instance.DespawnObject(collision.gameObject);
                    break;
                case "Player":
                    if (player.PlayerEngine.IsDashing)
                    {
                        var hittedPlayer = collision.gameObject.GetComponentInParent<Player>();
                        hittedPlayer.TakeDamage(player, hitDirection, new Vector2(-40, 15), 0);
                    }
                    break;
                case "Killzone":
                    player.TakeDamage(player,hitDirection, Vector2.zero, 3);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.CurrentState == PlayerState.NORMAL)
        {
            var hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            switch (collision.tag)
            {
                case "Spike":

                    player.TakeDamage(player, hitDirection, knockbackForce, 1);
                    break;
            }
        }
    }
}
