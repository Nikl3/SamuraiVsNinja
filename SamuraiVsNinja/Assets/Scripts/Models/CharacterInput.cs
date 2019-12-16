using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class CharacterInput : MonoBehaviour
    {
        #region VARIABLES

        private CharacterEngine characterEngine;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            characterEngine = GetComponent<CharacterEngine>();
        }

        private void Start()
        {
            RegisterInputs();
        }

        private void RegisterInputs()
        {
            InputManager.Instance.InputActions.Player.Jump.performed += context => characterEngine.OnJumpInputDown();

            InputManager.Instance.InputActions.Player.Jump.canceled += context => characterEngine.OnJumpInputUp();

            InputManager.Instance.InputActions.Player.Attack.performed += context => characterEngine.OnAttackInputDown();

            InputManager.Instance.InputActions.Player.Throw.performed += context => characterEngine.OnThrowInputDown();

            InputManager.Instance.InputActions.Player.Dash.performed += context => characterEngine.OnDashInputDown();

            InputManager.Instance.InputActions.Player.Movement.performed += context => characterEngine.OnMoveDown(InputManager.Instance.GetMovementInput);

            InputManager.Instance.InputActions.Player.Movement.canceled += context => characterEngine.OnMoveUp();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}

