using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region VARIABLES

    private Player player;
   

    #endregion VARIABLES

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.CurrentState != PlayerState.RESPAWN)
        {
            UpdateLocalInputs();
        }
    }

    public void UpdateLocalInputs()
    {
            Vector2 directionalInput = new Vector2(
            InputManager.Instance.GetHorizontalAxisRaw(player.PlayerData.ID),
            InputManager.Instance.GetVerticalAxisRaw(player.PlayerData.ID)
            );

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
            player.PlayerEngine.HandleMeleeAttacks();
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
