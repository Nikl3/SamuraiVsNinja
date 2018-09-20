﻿using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	private Animator animator;
	public Transform PlayerGraphics
	{
		get;
		private set;
	}
	private Transform ProjectileSpawnPoint;

	private void Awake ()
	{
		animator = GetComponentInChildren<Animator>();
		PlayerGraphics = transform;
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
		Fabric.EventManager.Instance.PostEvent(soundName);
	}

	public void ThrowKeyEvent()
	{
		LevelManager.Instance.SpawnProjectile(PlayerGraphics, ProjectileSpawnPoint.position);
	}
}
