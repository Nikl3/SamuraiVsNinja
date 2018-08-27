public interface IActions
{
    bool IsEnabled { get; set; }
    bool GetButtonDown(int playerId, InputAction action);
    bool GetButton(int playerId, InputAction action);
    bool GetButtonUp(int playerId, InputAction action);
    float GetAxis(int playerId, InputAction action);
}
