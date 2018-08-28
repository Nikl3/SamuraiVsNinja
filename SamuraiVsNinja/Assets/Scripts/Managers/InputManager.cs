using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseInputManager
{
    [SerializeField]
    private string playerAxisPrefix = "";
    [SerializeField]
    private int maxNumberOfPlayers = 4;


    [Header("Unity Axis Mappings")]
    [SerializeField]
    private string MoveHorizontalAxis = "Horizontal";
    [SerializeField]
    private string AttackAxis = "Attack";
    [SerializeField]
    private string ThrowAxis = "Throw";
    [SerializeField]
    private string DashAxis = "Dash";
    [SerializeField]
    private string ActionAxis = "Action";
    [SerializeField]
    private string CancelAxis = "Cancel";
    [SerializeField]
    private string SubmitAxis = "Submit";

    private Dictionary<int, string>[] actions;
    
    protected override void Awake()
    {
        actions = new Dictionary<int, string>[maxNumberOfPlayers];

        for (int i = 0; i < maxNumberOfPlayers; i++)
        {
            Dictionary<int, string> playerActions = new Dictionary<int, string>();
            actions[i] = playerActions;
            string prefix = !string.IsNullOrEmpty(playerAxisPrefix) ? playerAxisPrefix + i : string.Empty;
            AddAction(InputAction.MoveHorizontal, prefix + MoveHorizontalAxis, playerActions);
            AddAction(InputAction.Attack, prefix + AttackAxis, playerActions);
            AddAction(InputAction.Throw, prefix + ThrowAxis, playerActions);
            AddAction(InputAction.Dash, prefix + DashAxis, playerActions);
            AddAction(InputAction.Action, prefix + ActionAxis, playerActions);
            AddAction(InputAction.Cancel, prefix + CancelAxis, playerActions);
            AddAction(InputAction.Submit, prefix + SubmitAxis, playerActions);
        }
    }

    private static void AddAction(InputAction action, string actionName, Dictionary<int, string> actions)
    {
        if (string.IsNullOrEmpty(actionName))
        {
            return;
        }

        actions.Add((int)action, actionName);
    }

    public override bool GetButton(int playerId, InputAction action)
    {
        bool value = Input.GetButton(actions[playerId][(int)action]);
        return value;
    }

    public override bool GetButtonDown(int playerId, InputAction action)
    {
        bool value = Input.GetButtonDown(actions[playerId][(int)action]);
        return value;
    }

    public override bool GetButtonUp(int playerId, InputAction action)
    {
        bool value = Input.GetButtonUp(actions[playerId][(int)action]);
        return value;
    }

    public override float GetAxis(int playerId, InputAction action)
    {
        float value = Input.GetAxisRaw(actions[playerId][(int)action]);
        return value;
    }
}
