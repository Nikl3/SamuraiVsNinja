using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	public Animator Animator { get; private set; }

	private void Awake ()
	{
		Animator = GetComponentInChildren<Animator>();
	}

	public void AnimatorSetBool(string animationName, bool isActive)
	{
		Animator.SetBool(animationName, isActive);
	}

	public void AnimatorSetTrigger(string animationName)
	{
		Animator.SetTrigger(animationName);
	}
}
