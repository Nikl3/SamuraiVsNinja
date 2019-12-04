using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	private Character character;
	private Animator animator;
	private Transform ProjectileSpawnPoint;

    public Transform CharacterGraphics
    {
        get;
        private set;
    }

    private void Awake ()
	{
		character = GetComponentInParent<Character>();
		animator = GetComponentInChildren<Animator>();
		CharacterGraphics = transform;
		transform.localScale = new Vector2(-1, 1);
		ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
	}

	public bool GetAnimaionState(string animaionStateTag)
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsTag(animaionStateTag);
	}

	public void AnimatorSetBool(string animationName, bool isActive)
	{
		animator.SetBool(animationName, isActive);
	}

	public void SetAnimationController(RuntimeAnimatorController runtimeAnimatorController)
	{
		animator.runtimeAnimatorController = runtimeAnimatorController;
	}

	public void DeactivateParameter(string AnimationParameter)
	{
		animator.SetBool(AnimationParameter, false);
	}

	public void AnimatorSetTrigger(string animationName)
	{
		animator.SetTrigger(animationName);
	}

	public void PlaySoundInAnimation(string soundName)
	{
        //EventManager.Instance.PostEvent(soundName);
        Debug.LogError("Should play sound: " + soundName);
	}

	public void ThrowKeyEvent(int projectileTypeIndex)
	{	   
		var projectile = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(3, projectileTypeIndex == 0 ? 0 : 1), ProjectileSpawnPoint.position, Quaternion.identity);
		projectile.GetComponent<Projectile>().ProjectileInitialize(character, (int)CharacterGraphics.localScale.x);       
	}
}
