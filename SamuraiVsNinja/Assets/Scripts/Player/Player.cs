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

	private Color defaultColor;
	private readonly float backgroundLightAlpha = 0.1f;

	#endregion VARIABLES

	#region PROPERTIES

	public PlayerState CurrentState
	{
		get;
		private set;
	}
	
	public PlayerData PlayerData
	{
		get;
		private set;
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
	public PlayerIndicator PlayerIndicator
	{
		get;
		private set;
	}
	public Sword Sword
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
		PlayerIndicator = transform.GetComponentInChildren<PlayerIndicator>();
		defaultColor = SpriteRenderer.color;
		BackgroundLightRenderer = AnimatorController.transform.Find("BackgroundLight").GetComponent<SpriteRenderer>();
	}
	private void Start()
	{
		ResetValues();
	}

	public void Initialize(PlayerData playerData, RuntimeAnimatorController runtimeAnimatorController)
	{
		PlayerData = playerData;

		gameObject.name = playerData.PlayerName;
		AnimatorController.SetAnimationController(runtimeAnimatorController);

		SetPlayerIndicator();
		SetBackgroundLight();

		PlayerData.PlayerInfo.SetPlayerInfoIcons();

		transform.position = LevelManager.Instance.GetSpawnPoint(playerData.ID - 1);
		CameraEngine.Instance.AddTarget(transform);
		ChangePlayerState(PlayerState.RESPAWN, true);
	}

	private void SetPlayerIndicator()
	{
		PlayerIndicator.ChangeTextVisuals("P" + PlayerData.ID, PlayerData.PlayerColor);
		PlayerIndicator.name = "Player " + PlayerData.ID + " Indicator";
	}

	private void LoseOnigiri()
	{
		PlayerData.PlayerInfo.OnigirisLost++;

		//var droppedOnigiri = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(1, 0), transform.position);
		//droppedOnigiri.GetComponent<Item>().enabled = false;
		//droppedOnigiri.GetComponent<Animator>().enabled = false;
	}
	private void Die(Player attacker)
	{
		if (this != attacker)
		{
			attacker.PlayerData.PlayerInfo.Kills++;
		}

		PlayerData.PlayerInfo.Deaths++;

		if (onigiris > 0)
		{
			LoseOnigiri();
		}

		ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 0), transform.position);
		ChangePlayerState(PlayerState.RESPAWN);
		Fabric.EventManager.Instance.PostEvent("Die");
	}

	private void SetBackgroundLight()
	{
		var playerColor = PlayerData.PlayerColor;
		BackgroundLightRenderer.color = new Color(playerColor.r, playerColor.g, playerColor.b, backgroundLightAlpha);
	}

	public void ResetValues()
	{
		PlayerTriggerController.gameObject.tag = "Player";
		healthPoints = 3;
		onigiris = 0;
		PlayerEngine.ResetVariables();
		PlayerData.PlayerInfo.UpdateHealthPoints(healthPoints);
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
		PlayerData.PlayerInfo.OnigirisPicked++;

		if (onigiris >= 10)
		{
			LevelManager.Instance.EndGame(PlayerData.PlayerName);
		}

		PlayerData.PlayerInfo.UpdateOnigiris(onigiris);
	}
	public void TakeDamage(Player attacker, Vector2 direction, Vector2 knockbackForce, int damage, int stunDuration = 0)
	{
		if (CurrentState == PlayerState.NORMAL)
		{
			attacker.PlayerData.PlayerInfo.TotalHits++;

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

			PlayerData.PlayerInfo.UpdateHealthPoints(healthPoints);
		}
	}
}
