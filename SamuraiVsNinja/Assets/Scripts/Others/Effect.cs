using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
	private new ParticleSystem particleSystem;
	private Coroutine effectCoroutine;

	private void Awake ()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	private void OnEnable()
	{
		effectCoroutine = null;
		DespawnUntilOver();
	}

	private void DespawnUntilOver()
	{
		if(effectCoroutine == null)
			effectCoroutine = StartCoroutine(IDespawnUntilOver());
	}

	private IEnumerator IDespawnUntilOver()
	{
		yield return new WaitUntil(() => particleSystem.isStopped);

		Destroy(gameObject);
	}
}
