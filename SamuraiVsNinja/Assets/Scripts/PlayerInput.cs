using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
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
        //if (!hasAuthority)
        //    return;

        SimpleWrap();
      
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(HORIZONTAL_AXIS), Input.GetAxisRaw(VERTICAL_AXIS));

        PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            PlayerEngine.OnJumpInputUp();
        }
    }

    private void SimpleWrap()
    {
        Vector2 playerPosition = transform.position;
        if (playerPosition.x <= -30)
        {
            transform.position = new Vector2(-playerPosition.x, playerPosition.y);
        }
        else if (transform.position.x >= 30)
        {
            transform.position = new Vector2(-playerPosition.x, playerPosition.y);
        }
    }
}
