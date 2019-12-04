using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    private Character owner;
    protected float projectileSpeed;
    protected float selfDestroyTime = 1.8f;
    protected int startDirection;
    private Vector2 knockbackForce = new Vector2(10, 20);


    private SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObject(gameObject);
    }

    public void ProjectileInitialize(Character owner, int projectileDirection)
    {
        this.owner = owner;
        startDirection = -projectileDirection;
        spriteRenderer.flipX = startDirection > 0 ? true : false;
        Invoke("Despawn", selfDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var hitDirection = collision.transform.position - transform.position;
        hitDirection = hitDirection.normalized;

        if (collision.CompareTag("Player")) {
            var hittedPlayer = collision.GetComponentInParent<Character>();
            hittedPlayer.TakeDamage(owner, -hitDirection, knockbackForce, 1, 1);
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 2), collision.transform.position);
        }
        if (collision.gameObject.layer.Equals(9))
        {
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 2), transform.position);
            ObjectPoolManager.Instance.DespawnObject(gameObject);
        }
    }

    protected virtual void Update()
    {
        LevelManager.Instance.TeleportObject(transform);
        transform.position += ((Vector3)new Vector2(startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
