using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    private Player owner;
    private Vector2 knockbackForce = new Vector2(10, 20);

    private void Awake()
    {
        owner = GetComponentInParent<Player>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.CurrentState == PlayerState.NORMAL)
        {
            var hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            switch (collision.tag)
            {
                case "Projectile":
                    var collisionGameObject = collision.gameObject;
                    print(owner.name);
                    owner.TakeDamage(owner, hitDirection, knockbackForce, 1, 1);

                    ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 2), collision.transform.position);
                    ObjectPoolManager.Instance.DespawnObject(collisionGameObject);                 
                    break;

                case "Onigiri":
                    Fabric.EventManager.Instance.PostEvent("Pickup");
                    owner.AddOnigiri(1);
                    ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 6), collision.transform.position);
                    ObjectPoolManager.Instance.DespawnObject(collision.gameObject);
                    break;

                case "Sushi":
                    Fabric.EventManager.Instance.PostEvent("Pickup");

                    print("+1 Health");

                    ObjectPoolManager.Instance.DespawnObject(collision.gameObject);
                    break;

                case "Player":
                    if (owner.PlayerEngine.IsDashing)
                    {
                        var hittedPlayer = collision.gameObject.GetComponentInParent<Player>();
                        hittedPlayer.TakeDamage(owner, hitDirection, new Vector2(-40, 15), 0);
                    }
                    break;

                case "Killzone":
                    owner.TakeDamage(owner,hitDirection, Vector2.zero, 3);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (owner.CurrentState == PlayerState.NORMAL)
        {
            switch (collision.tag)
            {            
                case "Spike":

                    var hitDirection = collision.transform.position - transform.position;
                    hitDirection = hitDirection.normalized;
                    owner.TakeDamage(owner, hitDirection, knockbackForce, 1);

                    break;
            }
        }
    }
}
