using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    #region VARIABLES

    private Character owner;
    //private bool isStunned = false;   

    #endregion VARIABLES

    private void Awake()
    {
        owner = GetComponent<Character>();
    }

    private void Update()
    {
        if (owner.CurrentState != CHARACTER_STATE.RESPAWN && UIManager_Old.Instance.CurrentPanel == null)
        {
            LevelManager.Instance.TeleportObject(transform);
            UpdateLocalInputs();         
        }
    }

    //private void StunReset()
    //{
    //    isStunned = false;
    //}

    //public void Stun(float stunDuration)
    //{
    //    isStunned = true;
    //    owner.PlayerEngine.ResetVariables();
    //    Invoke("StunReset", stunDuration);
    //}

    public void UpdateLocalInputs()
    {
        //if (isStunned)
        //{
        //    return;
        //}
        
        Vector2 directionalInput = new Vector2(
        InputManager.Instance.GetHorizontalAxisRaw(owner.PlayerData.ID),
        InputManager.Instance.GetVerticalAxisRaw(owner.PlayerData.ID)
        );

        owner.CharacterEngine.SetDirectionalInput(directionalInput);

        if (InputManager.Instance.A_ButtonDown(owner.PlayerData.ID) && directionalInput.y != -1)
        {
            owner.CharacterEngine.OnJumpInputDown();
        }

        if (InputManager.Instance.A_ButtonUp(owner.PlayerData.ID))
        {
            owner.CharacterEngine.OnJumpInputUp();
        }

        if (InputManager.Instance.B_ButtonDown(owner.PlayerData.ID))
        {
            owner.CharacterEngine.HandleMeleeAttacks();
        }

        if (InputManager.Instance.GetRangeAttackAxisRaw(owner.PlayerData.ID) <= -1)
        {
            owner.CharacterEngine.OnThrow();
        }

        if (InputManager.Instance.GetDashAxisRaw(owner.PlayerData.ID) >= 1) 
        {
            if (directionalInput != Vector2.zero)
            {
                owner.CharacterEngine.OnDash();
            }
        }   
         
        owner.CharacterEngine.CalculateMovement();
    }
}
