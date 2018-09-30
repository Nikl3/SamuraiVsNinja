using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Player player;
    protected float projectileSpeed;
    protected float selfDestroyTime = 1.8f;
    protected int startDirection;

    private SpriteRenderer spriteRenderer;
    private Vector2 knockbackForce = new Vector2(20, 10);

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObject(gameObject);
    }

    public void ProjectileInitialize(Player owner, int projectileDirection)
    {
        startDirection = -projectileDirection;
        spriteRenderer.flipX = startDirection > 0 ? true : false;
        Invoke("Despawn", selfDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(9))
        {
            ObjectPoolManager.Instance.DespawnObject(gameObject);
        }
    }

    protected virtual void Update()
    {
        LevelManager.Instance.TeleportObject(transform);
        transform.position += ((Vector3)new Vector2(startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
