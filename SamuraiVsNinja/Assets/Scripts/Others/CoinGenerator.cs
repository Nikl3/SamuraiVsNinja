using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
	private LayerMask coinLayerMask;
	[SerializeField]
	private Transform[] Spawnpoints;
	private GameObject coinPrefab;
	private float SpawnIntervall = 0;

	private void Awake()
	{
		coinLayerMask = LayerMask.GetMask("Collectable");
		coinPrefab = ResourceManager.Instance.GetPrefabByName("Coin");
	}
	private void Update ()
	{
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{
			int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			Vector2 randomPosition = Spawnpoints[randomPositionIndex].position;

			if (!Physics2D.OverlapCircle(randomPosition, 2f, coinLayerMask))
			{
				Instantiate(coinPrefab, randomPosition, Quaternion.identity);
				SpawnIntervall = 0;
			}
		}
	}
}