﻿using System.Collections;
using UnityEngine;

public enum PlayerState {Normal, Inactive }

public class Player : MonoBehaviour
{
	#region VARIABLES

	private PlayerInfo playerInfo;
	private PlayerData playerData;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private CharacterController2D controller2D;
	private PlayerInput playerInput;
	private PlayerEngine playerEngine;
	private Sword sword;
    [SerializeField]
    private float flashTime;
    [SerializeField]
    private float flashSpeed;
    [SerializeField]
    private Color flashColor;
    private Color defaultColor;

    public PlayerState CurrentState = PlayerState.Normal;

	#endregion VARIABLES

	#region PROPERTIES

	public Sword Sword
	{
		get
		{
			return sword;
		}
	}

	public PlayerInfo PlayerInfo
	{
		get
		{
			return playerInfo;
		}
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
		get
		{
			return playerEngine;
		}
	}
	public PlayerInput PlayerInput
	{
		get
		{
			return playerInput;
		}
	}
	public CharacterController2D Controller2D
	{
		get
		{
			return controller2D;
		}        
	}

	public Animator Animator
	{
		get
		{
			return animator;
		}
	}
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return spriteRenderer;
		}
	}

	#endregion PROPERTIES

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		playerEngine = GetComponent<PlayerEngine>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
		controller2D = GetComponent<CharacterController2D>();
		sword = GetComponent<Sword>();
        defaultColor = spriteRenderer.color;
	}

	public void Initialize(PlayerData playerData, PlayerInfo playerInfo)
	{
		this.playerData = playerData;
		this.playerInfo = playerInfo;

		gameObject.name = playerData.PlayerName;
		playerInfo.PlayerName = playerData.PlayerName;
	}

    IEnumerator IReturnState(float flashSpeed, float flashTime) {
        CurrentState = PlayerState.Inactive;
        float currentTime = 0f;
        while (currentTime <= flashTime) {
            currentTime += Time.unscaledDeltaTime;
            Debug.Log(currentTime);
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.color = defaultColor;
            yield return new WaitForSeconds(flashSpeed);
        }
        currentTime = 0f;
        CurrentState = PlayerState.Normal;

        yield return null;
    }

    public void ReturnState() {
        StartCoroutine(IReturnState(flashSpeed, flashTime));
    }

    public void ReturnState(float flashTime) {
        StartCoroutine(IReturnState(flashSpeed, flashTime));
    }
}
