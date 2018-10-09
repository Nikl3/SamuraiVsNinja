using UnityEngine;

public class Onigiri : Item
{
    public void ShootOnigiri(Vector2 force)
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        tag = itemTag;
        transform.position = Vector2.one;
        graphics.position = Vector2.one;

        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.velocity = Vector2.zero;
    }

    protected override void Update()
    {
        base.Update();

        if (!isFloating)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Spawning"))
            {
                isFloating = true;
                tag = itemTag;
            }
        }
        else
        {
            if (currentLifetime > 0)
            {
                currentLifetime -= Time.deltaTime;
            }
            else
            {
                ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 6), transform.position);
                ObjectPoolManager.Instance.DespawnObject(gameObject);
            }
        }
    }
}
