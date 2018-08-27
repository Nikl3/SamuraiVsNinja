using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
	public Transform[] Spawnpoints;
	public GameObject coinPrefab;
	public float SpawnIntervall = 0;
    GameObject b;

    private void CoinDestroy() {
        Destroy(b);
    }

    private void OnEnable() {
        Invoke("CoinDestroy", 6f);
    }

    private void Update () {
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{

            int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			Vector2 randomPosition = Spawnpoints[randomPositionIndex].position;
            if (Physics2D.OverlapCircle(randomPosition, 2f)) {
                b = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
                SpawnIntervall = 0;
            }
		}
	}
}