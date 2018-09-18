using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singelton<InputManager>
{
    private EventSystem eventSystem;

    #region AXIS_INPUTS

    public float GetHorizontalAxisRaw(int id)
    {
        return Input.GetAxisRaw("Horizontal_J" + id);
    }

    public float GetVerticalAxisRaw(int id)
    {
        return Input.GetAxisRaw("Vertical_J" + id);
    }

    public float GetDashAxisRaw(int id)
    {
        return Input.GetAxisRaw("Dash_J" + id);
    }

    public float GetRangeAttackAxisRaw(int id)
    {
        return Input.GetAxisRaw("RangeAttack_J" + id);
    }

    #endregion AXIS_INPUTS

    #region BUTTON_INPUTS

    public bool Start_ButtonDown(int id)
    {
        return Input.GetButtonDown("Action_J" + id);
    }

    public bool Y_ButtonDown(int id) {
        return Input.GetButtonDown("Cancel_J" + id);
    }

    public bool B_ButtonDown(int id)
    {
        return Input.GetButtonDown("MeleeAttack_J" + id);
    }

    public bool A_ButtonDown(int id)
    {
        return Input.GetButtonDown("Jump_J" + id);
    }

    public bool A_ButtonUp(int id)
    {
        return Input.GetButtonUp("Jump_J" + id);
    }

    public bool X_ButtonDown(int id) {
        return Input.GetButtonDown("Grab_J" + id);
    }

    public bool X_ButtonUp(int id) {
        return Input.GetButton("Grab_J" + id);
    }

    #endregion BUTTON_INPUTS

    #region VARIABLES

    [SerializeField]
    private GameObject previousSelectedObject;

    [Header("Connected joysticks")]
    [SerializeField]
    private string[] joystickNames;

    #endregion VARIABLES

    private void Start()
    {
        eventSystem = EventSystem.current;
        joystickNames = Input.GetJoystickNames();   
    }

    public void FocusMenuPanel()
    {
        if (Input.anyKeyDown && eventSystem.currentSelectedGameObject == null)
        {
            ChangeActiveSelectedObject(previousSelectedObject);
        }
    }

    public void ChangeActiveSelectedObject(GameObject newSelectedObject)
    {
        previousSelectedObject = eventSystem.currentSelectedGameObject;

        if (!eventSystem.alreadySelecting)
        {
            eventSystem.SetSelectedGameObject(newSelectedObject);
        }
    }
}
