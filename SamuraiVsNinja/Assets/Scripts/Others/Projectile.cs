using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float selfDestroyTime = 5f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private int startDirection;

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void ProjectileMove(int projectileDirection)
    {
        startDirection = projectileDirection;
        spriteRenderer.flipX = startDirection > 0 ? true : false;
        Invoke("SelfDestroy", selfDestroyTime);
    }

    private void Update ()
    {
        transform.position +=  ((Vector3) new Vector2 (startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
