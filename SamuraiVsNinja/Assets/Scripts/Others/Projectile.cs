using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float selfDestroyTime = 5f;

    private int startDirection;

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void BulletMove(int projectileDirection)
    {
        startDirection = projectileDirection;
        Invoke("SelfDestroy", selfDestroyTime);
    }

    private void Update ()
    {
        transform.position +=  ((Vector3) new Vector2 (startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
