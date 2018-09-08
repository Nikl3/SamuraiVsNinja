using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region VARIABLES

    private Player player;
    private Transform playerGraphics;

    #endregion VARIABLES

    #region PROPERTIES


    #endregion PROPERTIES

    private void Awake()
    {
        player = GetComponent<Player>();
        playerGraphics = transform.Find("PlayerGraphics");
    }

    private void Update()
    {
        UpdateLocalInputs();
    }
  
    public void UpdateLocalInputs()
    {
        Vector2 directionalInput = new Vector2 (
            InputManager.Instance.GetHorizontalAxisRaw(player.PlayerData.ID),
            InputManager.Instance.GetVerticalAxisRaw(player.PlayerData.ID)
            );

        playerGraphics.localScale = new Vector2(player.Controller2D.Collisions.FaceDirection > 0 ? -1 : 1, 1);

        player.PlayerEngine.SetDirectionalInput(directionalInput);

        if (InputManager.Instance.A_ButtonDown(player.PlayerData.ID))
        {
            player.PlayerEngine.OnJumpInputDown();
        }

        if (InputManager.Instance.A_ButtonUp(player.PlayerData.ID))
        {
            player.PlayerEngine.OnJumpInputUp();
        }

        if (InputManager.Instance.B_ButtonDown(player.PlayerData.ID))
        {
            player.PlayerEngine.OnMeleeAttack();
        }

        if (InputManager.Instance.GetRangeAttackAxisRaw(player.PlayerData.ID) <= -1)
        {
            player.PlayerEngine.OnRangedAttack();
        }

        if (InputManager.Instance.GetDashAxisRaw(player.PlayerData.ID) >= 1)
        {
            if (directionalInput != Vector2.zero)
            {
                player.PlayerEngine.OnDash();
            }
        }

        player.PlayerEngine.CalculateMovement();
    }
}
