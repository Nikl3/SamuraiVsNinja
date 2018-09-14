using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float projectileSpeed = 60f;
    protected float selfDestroyTime = 5f;
    private SpriteRenderer spriteRenderer;
    private int startDirection;
    private Vector2 knockbackForce = new Vector2(20, 10);

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameObject.name = spriteRenderer.sprite.name;
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void ProjectileInitialize(int projectileDirection)
    {
        startDirection = -projectileDirection;
        spriteRenderer.flipX = startDirection > 0 ? true : false;
        Invoke("SelfDestroy", selfDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionGameObject = collision.gameObject;

        if (collisionGameObject.CompareTag("Player"))
        {
            var hittedPlayer = collision.GetComponent<Player>();

            if(hittedPlayer != null)
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection.x = -hitDirection.x;
                hitDirection = hitDirection.normalized;
                hittedPlayer.TakeDamage(hitDirection, knockbackForce, 1);
            }
           
            Destroy(gameObject);
        }
        else if (collisionGameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += ((Vector3)new Vector2(startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
