using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singelton<InputManager>
{
    private EventSystem eventSystem;

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

    [Header("Panel default/start objects")]
    [SerializeField]
    private GameObject[] panelDefaultSelectedObects;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        firstSelectedObject = eventSystem.firstSelectedGameObject;
        eventSystem.firstSelectedGameObject = firstSelectedObject;
    }

    private void Update()
    {
        Debug.DrawRay(Input.mousePosition, transform.forward * 200, Color.red);
    }

    public void ChangeActiveSelectedObject(int newSelectedObjectIndex)
    {
        previousSelectedObject = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(panelDefaultSelectedObects[newSelectedObjectIndex]);
    }

    public void ChangeToPreviousSelectedObject()
    {
        eventSystem.SetSelectedGameObject(previousSelectedObject);
    }
}
