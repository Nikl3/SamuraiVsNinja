﻿using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Player player;
    protected float projectileSpeed;
    protected float selfDestroyTime = 1.8f;
    private GameObject hitEffect;
    private SpriteRenderer spriteRenderer;
    private int startDirection;
    private Vector2 knockbackForce = new Vector2(20, 10);

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hitEffect = ResourceManager.Instance.GetPrefabByIndex(5, 2);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void ProjectileInitialize(Player player, int projectileDirection)
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
            var hittedPlayer = collision.GetComponentInParent<Player>();

            if(hittedPlayer != null)
            {
                var hitDirection = collision.transform.position - transform.position;
                hitDirection.x = -hitDirection.x;
                hitDirection = hitDirection.normalized;
                hittedPlayer.TakeDamage(player, hitDirection, knockbackForce, 1);

                Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            }
           
            Destroy(gameObject);
        }
        if (collisionGameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        LevelManager.Instance.TeleportObject(transform);
        transform.position += ((Vector3)new Vector2(startDirection, 0)) * projectileSpeed * Time.deltaTime;
    }
}
