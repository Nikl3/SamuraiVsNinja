using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Effect : MonoBehaviour
    {
        protected new ParticleSystem particleSystem;
        protected Coroutine effectCoroutine;

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            StartLifetime();
        }

        private void StartLifetime()
        {
            if(effectCoroutine != null)
            {
                return;
            }

            effectCoroutine = StartCoroutine(IDespawnUntilOver());
        }

        private IEnumerator IDespawnUntilOver()
        {
            yield return new WaitUntil(() => particleSystem.isStopped /*|| GameManager.Instance.IsLoadingScene*/);
            ObjectPoolManager.Instance.Despawn(this);

            effectCoroutine = null;
        }
    }
}