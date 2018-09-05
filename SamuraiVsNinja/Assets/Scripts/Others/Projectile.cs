using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float selfDestroyTime = 5f;
    private SpriteRenderer spriteRenderer;
    private int startDirection;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

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



    void OnTriggerEnter2D(Collider2D collision)  {
        if (collision.gameObject.CompareTag("Player")) {
            print(collision.gameObject.name);
            var hittedPlayer = collision.GetComponent<Player>();
            hittedPlayer.PlayerInfo.TakeDMG();
        }
    }

    void Update() {
        transform.position += ((Vector3)new Vector2(startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
