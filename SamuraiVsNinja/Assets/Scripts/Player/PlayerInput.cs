using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region VARIABLES

    private string ActionButton;
    private string HorizontalAxis;
    private string VerticalAxis;
    private string JumpButton;
    private string AttackButton;
    private string DashButton;

    private bool isLocalPlayer = false;
    private bool isGameRunning = false;

    private PlayerData playerData;
    private SpriteRenderer spriteRenderer;

    #endregion VARIABLES

    #region PROPERTIES

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerData = InputManager.Instance.GetPlayerData();
    }

    private void Start()
    {
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
            spriteRenderer.color = playerData.RandomColor;
        }
    }

    private void Update()
    {
        if (!isGameRunning)
            UpdateMenuInputs();

        if (isLocalPlayer)
            UpdateLocalInputs();
    }

    public void UpdateMenuInputs()
    {
        if (Input.GetButtonDown(JumpButton))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp(JumpButton))
        {
            PlayerEngine.OnJumpInputUp();
        }

        if (Input.GetButtonDown(AttackButton))
        {
            PlayerEngine.OnAttack();
        }

        var dashAxis = Input.GetAxisRaw(DashButton);

        if (dashAxis >= 1)
        {
            
        }
    }

    public void UpdateLocalInputs()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(HorizontalAxis), Input.GetAxisRaw(VerticalAxis));

        PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown(JumpButton))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp(JumpButton))
        {
            PlayerEngine.OnJumpInputUp();
        }

        if (Input.GetButtonDown(AttackButton))
        {
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
