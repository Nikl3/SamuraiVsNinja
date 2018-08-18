using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
	public Transform[] Spawnpoints;
	public GameObject coinPrefab;
	public float SpawnIntervall = 0;
	
	private void Update ()
	{
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{

			int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			Vector2 randomPosition = Spawnpoints[randomPositionIndex].position;

			Instantiate(coinPrefab, randomPosition, Quaternion.identity);
			SpawnIntervall = 0;
		}
	}
}