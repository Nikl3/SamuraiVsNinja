public abstract class BaseInputManager : SingeltonPersistant<BaseInputManager>, IActions
{
	public virtual bool IsEnabled
	{
		get
		{
			return isActiveAndEnabled;
		}

		set
		{
			enabled = value;
		}
	}

	public abstract bool GetButton(int playerId, InputAction action);
	public abstract bool GetButtonDown(int playerId, InputAction action);
	public abstract bool GetButtonUp(int playerId, InputAction action);
	public abstract float GetAxis(int playerId, InputAction action);
}
