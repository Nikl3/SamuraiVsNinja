using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	private Animator animator;

	private void Awake ()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void AnimatorSetBool(string animationName, bool isActive)
	{
		animator.SetBool(animationName, isActive);
	}

	public void AnimatorSetTrigger(string animationName)
	{
		animator.SetTrigger(animationName);
	}
}
