using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region VARIABLES

    private Transform playerInfoContainer;

    private string ActionButton;
    private string HorizontalAxis;
    private string VerticalAxis;
    private string JumpButton;
    private string AttackButton;
    private string DashButton;

    private bool isLocalPlayer = false;

    private PlayerInfo playerInfo;
    private PlayerData playerData;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterController2D controller2D;

    #endregion VARIABLES

    #region PROPERTIES

    public PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo;
        }
    }
    public PlayerEngine PlayerEngine
    {
        get;
        private set;
    }
    public CharacterController2D PlayerCharacterController2D
    {
        get;
        private set;
    }
  
    #endregion PROPERTIES

    private void Awake()
    {
        PlayerEngine = GetComponent<PlayerEngine>();
        PlayerCharacterController2D = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        controller2D = GetComponent<CharacterController2D>();
        playerInfoContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");
    }

    private void Start()
    {
        playerData = PlayerDataManager.Instance.GetPlayerData();

        if (transform.tag == "LocalPlayer")
        {            
            isLocalPlayer = true;

            ActionButton = playerData.ActionButton;
            HorizontalAxis = playerData.HorizontalAxis;
            VerticalAxis = playerData.VerticalAxis;
            JumpButton = playerData.JumpButton;
            AttackButton = playerData.AttackButton;
            DashButton = playerData.DashButton;

            gameObject.name = playerData.PlayerName;

            playerInfo = Instantiate(ResourceManager.Instance.GetPrefabByName("PlayerInfo").GetComponent<PlayerInfo>());
            playerInfo.transform.SetParent(playerInfoContainer);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
            UpdateLocalInputs();
    }
  
    public void UpdateLocalInputs()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));
        spriteRenderer.flipX = controller2D.Collisions.FaceDirection > 0 ? true : false;
        animator.SetBool("IsRunning", Mathf.Abs(directionalInput.x) > 0 ? true : false);

        PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown(JumpButton))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp(JumpButton))
        {
            PlayerEngine.OnJumpInputUp();
        }

        if (Input.GetButton/*Down*/(AttackButton))
        {
            print("ATTACK");
            PlayerEngine.OnAttack();
        }

        var dashAxis = Input.GetAxisRaw(DashButton);

        if (dashAxis >= 1)
        {
           if(directionalInput != Vector2.zero)
            PlayerEngine.OnDash();
        }

        PlayerEngine.CalculateMovement();
    }
}
