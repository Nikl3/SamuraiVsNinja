using System.Collections;
using UnityEngine;

public enum PlayerState {Normal, Inactive }

public class Player : MonoBehaviour
{
	#region VARIABLES
	private PlayerData playerData;
	[SerializeField]
	private readonly float flashTime;
	[SerializeField]
	private readonly float flashSpeed;
	[SerializeField]
	private Color flashColor;
	private Color defaultColor;

	public PlayerState CurrentState = PlayerState.Normal;

	#endregion VARIABLES

	#region PROPERTIES

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

	public Animator Animator
	{
		get;
		private set;
	}
	public SpriteRenderer SpriteRenderer
	{ get;
		private set;
	}

	#endregion PROPERTIES

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		PlayerEngine = GetComponent<PlayerEngine>();
		SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		Animator = GetComponentInChildren<Animator>();
		Controller2D = GetComponent<CharacterController2D>();
		Sword = GetComponent<Sword>();
		defaultColor = SpriteRenderer.color;
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo)
	{
		this.playerData = playerData;
		PlayerInfo = playerInfo;

		gameObject.name = playerData.PlayerName;
		playerInfo.PlayerName = playerData.PlayerName;
	}

	public void ReturnState()
	{
		StartCoroutine(IReturnState(flashSpeed, flashTime));
	}

	public void ReturnState(float flashTime)
	{
		StartCoroutine(IReturnState(flashSpeed, flashTime));
	}

	private IEnumerator IReturnState(float flashSpeed, float flashTime)
	{
		CurrentState = PlayerState.Inactive;
		float currentTime = 0f;
		while (currentTime <= flashTime)
		{
			currentTime += Time.unscaledDeltaTime;
			SpriteRenderer.color = flashColor;
			yield return new WaitForSeconds(flashSpeed);
			SpriteRenderer.color = defaultColor;
			yield return new WaitForSeconds(flashSpeed);
		}
		currentTime = 0f;
		CurrentState = PlayerState.Normal;

		yield return null;
	}
}
