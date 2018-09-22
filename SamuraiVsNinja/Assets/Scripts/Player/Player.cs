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

	private int healthPoints;
	private int onigiris;

	private PlayerData playerData;
	private Color flashColor = new Color(1, 1, 1, 0.5f);
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

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo)
	{
		this.playerData = playerData;
		PlayerInfo = playerInfo;
		PlayerInfo.Owner = this;

		gameObject.name = playerData.PlayerName;
	}

	public void ChangePlayerState(PlayerState newPlayerState)
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

				ResetValues();
				SpriteRenderer.color = new Color(1, 1, 1, 0.2f);
				PlayerEngine.Respawn(LevelManager.Instance.RandomSpawnPoint());

				break;

			default:

				CurrentState = PlayerState.NORMAL;

				break;
		}
	}

	public void AddOnigiri(int amount)
	{
		onigiris += amount;

		if (onigiris >= 1)
		{
			LevelManager.Instance.Victory(playerData.PlayerName);
		}

		PlayerInfo.UpdateOnigiris(onigiris);
	}

	public void TakeDamage(Vector2 direction, Vector2 knockbackForce, int damage)
	{
		if (CurrentState == PlayerState.NORMAL)
		{
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
				Die();
			}

			PlayerInfo.UpdateHealthPoints(healthPoints);
		}
	}

	private void DropOnigiri()
	{
			Instantiate(
				ResourceManager.Instance.GetPrefabByIndex(1, 0),
				transform.position,
				Quaternion.identity
				);
	}

	private void Die() {
		if (onigiris > 0) {
			DropOnigiri();
		}
		Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 0), transform.position, Quaternion.identity);
		ChangePlayerState(PlayerState.RESPAWN);
		Fabric.EventManager.Instance.PostEvent("Die");
	}
}
