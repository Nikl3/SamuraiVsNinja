using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	private Animator animator;
    private Player player;

	private void Awake ()
	{
		animator = GetComponentInChildren<Animator>();
        player = GetComponentInParent<Player>();
	}

	public void AnimatorSetBool(string animationName, bool isActive)
	{
		animator.SetBool(animationName, isActive);
	}

	public void AnimatorSetTrigger(string animationName)
	{
		animator.SetTrigger(animationName);
	}

    public void ChangeAttackTrue()
    {
        player.PlayerEngine.IsAttacking = true;
    }

    public void ChangeAttackFalse()
    {
        player.PlayerEngine.IsAttacking = false;
    }

    public void PlaySoundInAnimation(string soundName)
    {
        Fabric.EventManager.Instance.PostEvent(soundName);
    }
}
