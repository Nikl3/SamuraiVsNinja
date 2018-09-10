using System.Collections;
using UnityEngine;

public class DespawnParticleEffect : MonoBehaviour
{
	private new ParticleSystem particleSystem;
	private Coroutine effectCoroutine;
	private float effectDuration;

	private void Awake ()
	{
		particleSystem = GetComponent<ParticleSystem>();

		effectDuration = particleSystem.main.duration;

	}

	private void OnEnable()
	{
		effectCoroutine = null;
		DespawnUntilOver(effectDuration);
	}

	private void DespawnUntilOver(float effectDuration)
	{
		if(effectCoroutine == null)
			effectCoroutine = StartCoroutine(IDespawnUntilOver(effectDuration));
	}

	private IEnumerator IDespawnUntilOver(float effectDuration)
	{
		yield return new WaitForSeconds(effectDuration);

		Destroy(gameObject);
	}
}
