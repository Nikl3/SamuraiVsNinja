using System.Collections;
using UnityEngine;

public enum PlayerState
{
	Normal,
	Inactive
}

public class Player : MonoBehaviour
{
	#region VARIABLES

	[SerializeField]
	public AudioClip[] PlayerAudioClips;

	private PlayerData playerData;
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

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();
		PlayerEngine = GetComponent<PlayerEngine>();
		SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		AnimatorController = GetComponentInChildren<AnimatorController>();
		Controller2D = GetComponent<CharacterController2D>();
		AudioSource = GetComponent<AudioSource>();
		Sword = GetComponent<Sword>();
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo)
	{
		this.playerData = playerData;
		PlayerInfo = playerInfo;

		gameObject.name = playerData.PlayerName;
		playerInfo.PlayerName = playerData.PlayerName;

        defaultColor = PlayerData.PlayerColor;

		CreatePlayerIndicator();
	}

	private void CreatePlayerIndicator()
	{
		var playerIndicator = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2));
		playerIndicator.GetComponent<PlayerIndicator>().PlayerIdText = "P" + playerData.ID;
		playerIndicator.transform.SetParent(transform);
		playerIndicator.transform.localPosition = new Vector2(0, 4);
	}

	public void PlayAudioClip(int audioIndex)
	{
		AudioSource.clip = PlayerAudioClips[audioIndex];
		//if(!AudioSource.isPlaying)
		AudioSource.Play();
	}

	public void ReturnState(float flashSpeed = 0.2f, float flashTime = 0.1f)
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
