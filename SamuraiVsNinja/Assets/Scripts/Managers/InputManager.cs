using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singelton<InputManager>
{
    public GameObject CurrentSelectedObject
    {
        get
        {
            return EventSystem.currentSelectedGameObject;
        }
    }

    private EventSystem eventSystem;
    public EventSystem EventSystem
    {
        get
        {
            if(eventSystem)
            {
                return eventSystem;
            }

            eventSystem = EventSystem.current;

            return eventSystem;
        }
    }

    public bool IsAnyKeyDown
    {
        get
        {
            return Input.anyKeyDown;
        }
    }

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

    public bool Y_ButtonDown(int id)
    {
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

    public bool X_ButtonDown(int id)
    {
        return Input.GetButtonDown("RangeAttack_J" + id);
    }

    public bool X_ButtonUp(int id)
    {
        return Input.GetButton("RangeAttack_J" + id);
    }

    #endregion BUTTON_INPUTS

    #region VARIABLES

    [Space]
    [Header("Connected Joysticks")]
    [SerializeField]
    private string[] joystickNames;

    #endregion VARIABLES

    private void Awake()
    {
        joystickNames = Input.GetJoystickNames();
    }

    public void FocusToButton(GameObject focusObject)
    {
        if (GetVerticalAxisRaw(1) != 0 && EventSystem.currentSelectedGameObject == null)
        {
            ChangeActiveSelectedObject(focusObject);
        }
    }

    public void ChangeActiveSelectedObject(GameObject newSelectedObject)
    {   
        if (!EventSystem.alreadySelecting)
        {
            EventSystem.SetSelectedGameObject(newSelectedObject);
        }
    }
}
