﻿using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Onigiri"))
        {
            player.PlayerInfo.ModifyCoinValues(1);
            Destroy(collision.gameObject);
        }
    }
}