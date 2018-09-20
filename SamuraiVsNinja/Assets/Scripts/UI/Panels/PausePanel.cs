using UnityEngine;

public class PausePanel : UIPanel
{
	public override void OpenBehaviour()
	{
		base.OpenBehaviour();
		Time.timeScale = 0f;
	}

	public override void CloseBehaviour()
	{
		base.CloseBehaviour();
		Time.timeScale = 1f;
	}
}
