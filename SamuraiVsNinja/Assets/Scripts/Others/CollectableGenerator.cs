using UnityEngine;

public class CollectableGenerator : MonoBehaviour
{
	private LayerMask collectableLayerMask;
	[SerializeField]
	private Transform[] Spawnpoints;
	private GameObject coinPrefab;
	private float SpawnIntervall = 0;

	private void Awake()
	{
		collectableLayerMask = LayerMask.GetMask("Collectable");
		coinPrefab = ResourceManager.Instance.GetPrefabByName("Onigiri");
	}
	private void Update ()
	{
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{
			int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			Vector2 randomPosition = Spawnpoints[randomPositionIndex].position;

			if (!Physics2D.OverlapCircle(randomPosition, 2f, collectableLayerMask))
			{
				Instantiate(coinPrefab, randomPosition, Quaternion.identity);
				SpawnIntervall = 0;
			}
		}
	}
}