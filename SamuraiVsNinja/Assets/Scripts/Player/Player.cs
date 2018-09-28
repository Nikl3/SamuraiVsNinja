﻿using UnityEngine;

public enum PlayerState
{
	NORMAL,
	INVINCIBILITY,
	RESPAWN
}

public class Player : MonoBehaviour
{
    #region VARIABLES
    
    public float RespawnDelay;

    private int healthPoints;
	private int onigiris;

	private PlayerData playerData;
	private Color defaultColor;
	private readonly float backgroundLightAlpha = 0.1f;

	#endregion VARIABLES

	#region PROPERTIES

	public PlayerState CurrentState
	{
		get;
		private set;
	}

	public Sword Sword
	{
		get;
		private set;
	}
	public PlayerInfo PlayerInfo
	{
		get;
		private set;
	}
	public PlayerData PlayerData
	{
		get
		{
			if (playerData == null)
				playerData = new PlayerData(1);

			return playerData;
		}
	}
	public PlayerEngine PlayerEngine
	{
		get;
		private set;
	}
	public PlayerInput PlayerInput
	{
		get;
		private set;
	}
	public CharacterController2D Controller2D
	{
		get;
		private set;
	}
	public PlayerTriggerController PlayerTriggerController
	{
		get;
		private set;
	}
	public AnimatorController AnimatorController
	{
		get;
		private set;
	}
	public SpriteRenderer SpriteRenderer
	{
		get;
		private set;
	}
	public SpriteRenderer BackgroundLightRenderer;

	#endregion PROPERTIES

	private void ResetValues()
	{
		PlayerTriggerController.gameObject.tag = "Player";
		healthPoints = 3;
		onigiris = 0;
		PlayerEngine.ResetVariables();
	}

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		PlayerEngine = GetComponent<PlayerEngine>();
		SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		AnimatorController = GetComponentInChildren<AnimatorController>();
		Controller2D = GetComponent<CharacterController2D>();
		PlayerTriggerController = GetComponentInChildren<PlayerTriggerController>();
		Sword = GetComponent<Sword>();
		defaultColor = SpriteRenderer.color;
		BackgroundLightRenderer = AnimatorController.transform.Find("BackgroundLight").GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		ResetValues();
		var playerColor = PlayerData.PlayerColor;
		BackgroundLightRenderer.color = new Color(playerColor.r, playerColor.g, playerColor.b, backgroundLightAlpha);
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo, RuntimeAnimatorController runtimeAnimatorController)
	{
		this.playerData = playerData;
		PlayerInfo = playerInfo;
		PlayerInfo.Owner = this;
		gameObject.name = playerData.PlayerName;

        AnimatorController.SetAnimationController(runtimeAnimatorController);

    }

	public void ChangePlayerState(PlayerState newPlayerState, bool firstSpawn = false)
	{
		CurrentState = newPlayerState;

		switch (CurrentState)
		{
			case PlayerState.NORMAL:
				PlayerTriggerController.gameObject.tag = "Player";
				SpriteRenderer.color = defaultColor;
				break;

			case PlayerState.INVINCIBILITY:
				PlayerTriggerController.gameObject.tag = "Untagged";
				SpriteRenderer.color = defaultColor;
				PlayerEngine.StartInvincibility(2f, 0.1f);
				break;

			case PlayerState.RESPAWN:
					   
				if (firstSpawn)
				{
					SpriteRenderer.color = new Color(1, 1, 1, 0f);
					PlayerEngine.Respawn(LevelManager.Instance.GetSpawnPoint(PlayerData.ID - 1), RespawnDelay);
					firstSpawn = false;
				}
				else
				{
					ResetValues();
					SpriteRenderer.color = new Color(1, 1, 1, 0.2f);
					PlayerEngine.Respawn(LevelManager.Instance.RandomSpawnPoint(), 0.5f);
				}

				break;

			default:

				CurrentState = PlayerState.NORMAL;

				break;
		}
	}

	public void AddOnigiri(int amount)
	{
		onigiris += amount;
		PlayerInfo.OnigirisPicked++;

		if (onigiris >= 3)
		{
			LevelManager.Instance.Victory(playerData.PlayerName);
		}

		PlayerInfo.UpdateOnigiris(onigiris);
	}

	public void TakeDamage(Player attacker, Vector2 direction, Vector2 knockbackForce, int damage)
	{
		if (CurrentState == PlayerState.NORMAL)
		{//disable CalculateMovement() 0,5s ajaksi
            PlayerInput.Stun(2);

            healthPoints -= damage;

			if (healthPoints >= 1)
			{
				if (damage > 0)
				{
					ChangePlayerState(PlayerState.INVINCIBILITY);
				}
				PlayerEngine.OnKnockback(direction, knockbackForce);
				Fabric.EventManager.Instance.PostEvent("HitContact");
			}
			else
			{
				Die(attacker);
			}

			PlayerInfo.UpdateHealthPoints(healthPoints);
		}
	}

	private void DropOnigiri()
	{
		PlayerInfo.OnigirisLost++;

		var droppedOni = Instantiate(ResourceManager.Instance.GetPrefabByIndex(1, 0), transform.position, Quaternion.identity);
        droppedOni.GetComponent<OnigiriFloater>().enabled = false;
        droppedOni.GetComponent<Animator>().enabled = false;
	}

	private void Die(Player attacker)
	{
		if(this != attacker) 
		{
			attacker.PlayerInfo.Kills++;
		}

		PlayerInfo.Deaths++;

		if (onigiris > 0)
		{
			DropOnigiri();
		}

		Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 0), transform.position, Quaternion.identity);
		ChangePlayerState(PlayerState.RESPAWN);
		Fabric.EventManager.Instance.PostEvent("Die");
	}
}
