using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class CharacterInput : MonoBehaviour
    {
        #region VARIABLES

        private CharacterEngine character;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            character = GetComponent<CharacterEngine>();
        }

        private void Start()
        {       
            InputManager.Instance.InputActions.Player.Jump.performed += context => character.OnJumpInputDown();

            InputManager.Instance.InputActions.Player.Jump.canceled += context => character.OnJumpInputUp();
        }

        private void Update()
        {
            var directionalInput = InputManager.Instance.GetMovementInput;

            character.SetDirectionalInput(directionalInput);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}

