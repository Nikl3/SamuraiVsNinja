using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
	public ParticleSystem ParticleSystem
	{
		get;
		private set;
	}

	private Coroutine effectCoroutine;

	private void Awake ()
	{
		ParticleSystem = GetComponent<ParticleSystem>();
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
		yield return new WaitUntil(() => ParticleSystem.isStopped || GameMaster.Instance.IsLoadingScene);
		ObjectPoolManager.Instance.DespawnObject(gameObject);
	}
}
