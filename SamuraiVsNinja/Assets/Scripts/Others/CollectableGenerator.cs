using UnityEngine;

public class CollectableGenerator : MonoBehaviour
{
	private LayerMask collectableLayerMask;
	[SerializeField]
	private Transform[] Spawnpoints;
	private GameObject onigiriPrefab;
	private float SpawnIntervall = 0;

	private void Awake()
	{
		collectableLayerMask = LayerMask.GetMask("Collectable");		
	}

	private void Start()
	{
		onigiriPrefab = ResourceManager.Instance.GetPrefabByIndex(1, 0);
	}

	private void Update ()
	{
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{
			int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			Vector2 randomPosition = Spawnpoints[randomPositionIndex].position;

            if (!Physics2D.OverlapCircle(randomPosition, 2f, collectableLayerMask)) {
                Instantiate(onigiriPrefab, randomPosition, Quaternion.identity);
                SpawnIntervall = 0;
            }
            SpawnIntervall = 0f;
		}
	}
}