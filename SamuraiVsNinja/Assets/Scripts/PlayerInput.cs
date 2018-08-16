using UnityEngine;

[RequireComponent(typeof(PlayerEngine))]
public class PlayerInput : Singelton<PlayerInput>
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    public PlayerEngine PlayerEngine { get; private set; }
    public CharacterController2D PlayerCharacterController2D { get; private set; }

    private void Awake()
    {
        PlayerEngine = GetComponent<PlayerEngine>();
        PlayerCharacterController2D = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        //if (Time.timeScale.Equals(0))
        //    return;

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(HORIZONTAL_AXIS), Input.GetAxisRaw(VERTICAL_AXIS));

        PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetMouseButtonDown(0))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlayerEngine.OnJumpInputUp();
        }
    }
}
