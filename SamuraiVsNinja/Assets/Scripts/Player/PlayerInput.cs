using UnityEngine;

public class PlayerInput : Singelton<PlayerInput>
{
    private PlayerData playerData;
    private bool isLocalPlayer = false;

    private string ActionButton = "Action";
    private string HorizontalAxis = "Horizontal";
    private string VerticalAxis = "Vertical";
    private string JumpButton = "Jump";
    private string AttackButton = "Attack";
    private string DashButton = "Dash";
    private int controllerNumber = 0;

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

    private void Awake()
    {
        PlayerEngine = GetComponent<PlayerEngine>();
        PlayerCharacterController2D = GetComponent<CharacterController2D>();      
    }

    private void Start()
    {
        playerData = InputManager.Instance.GetCorrectPlayerData(gameObject.name);

        if (transform.tag == "LocalPlayer")
        {
            isLocalPlayer = true;
            // Keyboard...
            SetControllerNumber(0);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
            UpdateLocalInputs();
    }

    public void SetControllerNumber(int number)
    {
        controllerNumber = number;

        if(number <= 0)
        {
            //Debug.LogWarning("Keyboard!");
            return;
        }

        ActionButton = "Action" + "_J" + controllerNumber;
        HorizontalAxis = "Horizontal" + "_J" + controllerNumber;
        VerticalAxis = "Vertical" + "_J" + controllerNumber;
        JumpButton = "Jump" + "_J" + controllerNumber;
        AttackButton = "Attack" + "_J" + controllerNumber;
        DashButton = "Dash" + "_J" + controllerNumber;
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

        if (Input.GetButtonDown(DashButton))
        {
           if(directionalInput != Vector2.zero)
            PlayerEngine.OnDash();
        }

        PlayerEngine.CalculateMovement();
    }
}
