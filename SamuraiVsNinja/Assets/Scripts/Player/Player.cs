using UnityEngine;

public enum PlayerState
{
	NORMAL,
	INVINCIBILITY,
	RESPAWN
}

public class Player : MonoBehaviour
{
	#region VARIABLES

	private readonly float firstRespawnDelay = 4f;
	private readonly float respawnDelay = 0.5f;

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
	public SpriteRenderer BackgroundLightRenderer
	{
		get;
		private set;
	}

	#endregion PROPERTIES

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		PlayerEngine = GetComponent<PlayerEngine>();
		SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		AnimatorController = GetComponentInChildren<AnimatorController>();
		Controller2D = GetComponent<CharacterController2D>();
		PlayerTriggerController = GetComponentInChildren<PlayerTriggerController>();
		Sword = GetComponentInChildren<Sword>();
		defaultColor = SpriteRenderer.color;
		BackgroundLightRenderer = AnimatorController.transform.Find("BackgroundLight").GetComponent<SpriteRenderer>();
	}
	private void Start()
	{
		ResetValues();
		var playerColor = PlayerData.PlayerColor;
		BackgroundLightRenderer.color = new Color(playerColor.r, playerColor.g, playerColor.b, backgroundLightAlpha);
	}

	private void LoseOnigiri()
	{
		PlayerInfo.OnigirisLost++;

		//var droppedOnigiri = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(1, 0), transform.position);
		//droppedOnigiri.GetComponent<Item>().enabled = false;
		//droppedOnigiri.GetComponent<Animator>().enabled = false;
	}
	private void Die(Player attacker)
	{
		if (this != attacker)
		{
			attacker.PlayerInfo.Kills++;
		}

		PlayerInfo.Deaths++;

		if (onigiris > 0)
		{
			LoseOnigiri();
		}

		ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 0), transform.position);
		ChangePlayerState(PlayerState.RESPAWN);
		Fabric.EventManager.Instance.PostEvent("Die");
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo, EndGameStats endGameStats, RuntimeAnimatorController runtimeAnimatorController)
	{
		this.playerData = playerData;
		playerData.SpawnedPlayer = this;
		PlayerInfo = playerInfo;
		PlayerInfo.Owner = this;
		PlayerInfo.EndGameStats = endGameStats;

		gameObject.name = playerData.PlayerName;

		AnimatorController.SetAnimationController(runtimeAnimatorController);
	}
	public void ResetValues()
	{
		PlayerTriggerController.gameObject.tag = "Player";
		healthPoints = 3;
		onigiris = 0;
		PlayerEngine.ResetVariables();
		PlayerInfo.UpdateHealthPoints(healthPoints);
	}
	public void ChangePlayerState(PlayerState newPlayerState, bool firstSpawn = false)
	{
		CurrentState = newPlayerState;

		switch (CurrentState)
		{
			case PlayerState.NORMAL:
				//Debug.LogError(name + " NORMAL");
				PlayerTriggerController.gameObject.tag = "Player";
				SpriteRenderer.color = defaultColor;
				break;

			case PlayerState.INVINCIBILITY:
				//Debug.LogError(name + " INVINCIBILITY");
				PlayerTriggerController.gameObject.tag = "Untagged";
				SpriteRenderer.color = defaultColor;
				PlayerEngine.StartInvincibility(2f, 0.1f);
				break;

			case PlayerState.RESPAWN:
				//Debug.LogError(name + " RESPAWN");
				if (firstSpawn)
				{
					SpriteRenderer.color = new Color(1, 1, 1, 0f);
					PlayerEngine.Respawn(LevelManager.Instance.GetSpawnPoint(PlayerData.ID - 1), firstRespawnDelay);
					firstSpawn = false;
				}
				else
				{
					SpriteRenderer.color = new Color(1, 1, 1, 0.2f);
					PlayerEngine.Respawn(LevelManager.Instance.RandomSpawnPosition(0), respawnDelay);
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
			LevelManager.Instance.EndGame(playerData.PlayerName);
		}

		PlayerInfo.UpdateOnigiris(onigiris);
	}
	public void TakeDamage(Player attacker, Vector2 direction, Vector2 knockbackForce, int damage, int stunDuration = 0)
	{
		if (CurrentState == PlayerState.NORMAL)
		{
			PlayerInput.Stun(stunDuration);

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
}
