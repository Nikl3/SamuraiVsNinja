using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region VARIABLES

    [SerializeField]
    private float rangeAttackAxis;
    [SerializeField]
    private float dashAxis;

    private Player player;

    #endregion VARIABLES

    #region PROPERTIES

  
    #endregion PROPERTIES

    private void Awake()
    {
        player = GetComponent<Player>();
    }


    private void Update()
    {
        UpdateLocalInputs();
    }
  
    public void UpdateLocalInputs()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(player.PlayerData.HorizontalAxis), Input.GetAxisRaw(player.PlayerData.VerticalAxis));
        player.SpriteRenderer.flipX = player.Controller2D.Collisions.FaceDirection > 0 ? true : false;
        player.Animator.SetBool("IsRunning", Mathf.Abs(directionalInput.x) > 0 ? true : false);
        player.PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown(player.PlayerData.JumpButton))
        {
            player.PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp(player.PlayerData.JumpButton))
        {
            player.PlayerEngine.OnJumpInputUp();
        }

        if (Input.GetButtonDown(player.PlayerData.MeleeAttackButton))
        {
            print("MELEE ATTACK");
            player.PlayerEngine.OnAttack();
        }

        rangeAttackAxis = Input.GetAxisRaw(player.PlayerData.RangeAttackButton);

        if (rangeAttackAxis >= 1)
        {
            if (directionalInput != Vector2.zero)
                player.PlayerEngine.OnRangedAttack();
        }

        dashAxis = Input.GetAxisRaw(player.PlayerData.DashButton);

        if (dashAxis >= 1)
        {
           if(directionalInput != Vector2.zero)
                player.PlayerEngine.OnDash();
        }

        player.PlayerEngine.CalculateMovement();
    }
}
