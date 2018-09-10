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

	[SerializeField]
	public AudioClip[] PlayerAudioClips;

	private PlayerData playerData;
	[SerializeField]
	private Color flashColor;
	// private Color defaultColor;

	public PlayerState CurrentState
	{
		get;
		private set;
	}

	#endregion VARIABLES

	#region PROPERTIES

	public Sword Sword
	{
		get;
		private set;
	}

	public AudioSource AudioSource
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

	#endregion PROPERTIES

	private void CreatePlayerIndicator()
	{
		var playerIndicator = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2));
		playerIndicator.GetComponent<PlayerIndicator>().PlayerIdText = "P" + playerData.ID;
		playerIndicator.transform.SetParent(transform);
		playerIndicator.transform.localPosition = new Vector2(0, 4);
	}

	private void ResetValues()
	{
		healthPoints = 3;
		onigiris = 0;
	}

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		PlayerEngine = GetComponent<PlayerEngine>();
		SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		AnimatorController = GetComponentInChildren<AnimatorController>();
		Controller2D = GetComponent<CharacterController2D>();
		AudioSource = GetComponent<AudioSource>();
		Sword = GetComponent<Sword>();
		// defaultColor = SpriteRenderer.color;
	}

	private void Start()
	{
		ResetValues();
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo)
	{
		this.playerData = playerData;
		PlayerInfo = playerInfo;

		gameObject.name = playerData.PlayerName;
		playerInfo.PlayerName = playerData.PlayerName;

		//defaultColor = PlayerData.PlayerColor;

		CreatePlayerIndicator();
	}

	public void ChangePlayerState(PlayerState newPlayerState)
	{
		CurrentState = newPlayerState;
	}

	public void PlayAudioClip(int audioIndex)
	{
		AudioSource.clip = PlayerAudioClips[audioIndex];
		//if(!AudioSource.isPlaying)
		AudioSource.Play();
	}

	public void AddOnigiri(int amount)
	{
		onigiris += amount;
		PlayerInfo.UpdateOnigiris(onigiris);
	}

	public void TakeDamage(Vector2 direction)
	{
		if (CurrentState == PlayerState.NORMAL)
		{
			healthPoints--;

			if (healthPoints >= 1)
			{

				PlayerEngine.Invincibility();

				PlayerEngine.OnKnockback(direction);
				PlayAudioClip(2);

				if (onigiris > 0)
					DropOnigiri();
			}
			else
			{
				//hittedPlayer.ChangePlayerState(PlayerState.RESPAWN);

				Die();
				ResetValues();
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

	private void Die()
	{
		PlayAudioClip(3);
		Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 0), transform.position, Quaternion.identity);
	}
}
