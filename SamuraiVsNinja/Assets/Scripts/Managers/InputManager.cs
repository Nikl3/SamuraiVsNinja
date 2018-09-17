using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singelton<InputManager>
{
    private EventSystem eventSystem;
    [SerializeField]
    private BaseInputModule baseInputModule;

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
        return Input.GetButtonDown("Grab_J" + id);
    }

    public bool X_ButtonUp(int id)
    {
        return Input.GetButton("Grab_J" + id);
    }

    #endregion BUTTON_INPUTS

    #region VARIABLES

    [Header("Event system variables")]
    [SerializeField]
    private GameObject firstSelectedObject;
    [SerializeField]
    private GameObject previousSelectedObject;
    public GameObject CurrentSeletedObject
    {
        get
        {
            return eventSystem.currentSelectedGameObject;
        }
        set
        {
            eventSystem.SetSelectedGameObject(value);
        }
    }
    public GameObject[] PanelDefaultSelectedObects
    {
        get
        {
            return panelDefaultSelectedObects;
        }

        set
        {
            panelDefaultSelectedObects = value;
        }
    }

    [Header("Panel default/start objects")]
    [SerializeField]
    private GameObject[] panelDefaultSelectedObects;

    [Header("Connected joysticks")]
    [SerializeField]
    private string[] joystickNames;

    #endregion VARIABLES

    private void Start()
    {
        eventSystem = EventSystem.current;
        firstSelectedObject = eventSystem.firstSelectedGameObject;
        eventSystem.firstSelectedGameObject = firstSelectedObject;
        joystickNames = Input.GetJoystickNames();   
    }

    public void ChangeActiveSelectedObject(int newSelectedObjectIndex)
    {
        previousSelectedObject = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(PanelDefaultSelectedObects[newSelectedObjectIndex]);
    }

    public void ChangeToPreviousSelectedObject()
    {
        eventSystem.SetSelectedGameObject(previousSelectedObject);
    }
}
