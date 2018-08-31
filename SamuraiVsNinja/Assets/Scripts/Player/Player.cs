using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
	#region VARIABLES

	private PlayerNetwork playerNetwork;
	private NetworkIdentity networkIdentity;

	private PlayerInfo playerInfo;
	private PlayerData playerData;
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private CharacterController2D controller2D;
	private PlayerInput playerInput;
	private PlayerEngine playerEngine;

	private Transform playerInfoContainer;

	#endregion VARIABLES

	#region PROPERTIES

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
        set
        {
            playerData = value;
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

	private void Awake ()
	{
		playerNetwork = GetComponent<PlayerNetwork>();
		networkIdentity = GetComponent<NetworkIdentity>();

		playerInput = GetComponent<PlayerInput>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		playerEngine = GetComponent<PlayerEngine>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
		controller2D = GetComponent<CharacterController2D>();
		playerInfoContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");

		if (MainNetworkManager.Instance == null)
		{
			Destroy(playerNetwork);
			Destroy(networkIdentity);
		}
	}

	private void Start()
	{
		//gameObject.name = playerData.PlayerName;
		playerInfo = Instantiate(ResourceManager.Instance.GetPrefabByName("PlayerInfo").GetComponent<PlayerInfo>());

		playerInfo.transform.SetParent(playerInfoContainer);

		playerInfo.transform.localScale = Vector3.zero;
		playerInfo.transform.localScale = Vector3.one;
	}
}
