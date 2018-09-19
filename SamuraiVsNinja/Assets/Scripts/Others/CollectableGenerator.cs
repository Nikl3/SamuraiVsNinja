using UnityEngine;

public class CollectableGenerator : MonoBehaviour
{
    [SerializeField]
    private float radius = 4f;
    [SerializeField]
    private LayerMask collisionLayerMask;
	[SerializeField]
	private Transform[] Spawnpoints;
	private GameObject onigiriPrefab;
	private float SpawnIntervall = 0;
    private Vector2 randomPosition;


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
			randomPosition = Spawnpoints[randomPositionIndex].position;

            if (!Physics2D.OverlapCircle(randomPosition, radius, collisionLayerMask)) {
                Instantiate(onigiriPrefab, randomPosition, Quaternion.identity);
                SpawnIntervall = 0;
            }
            SpawnIntervall = 0f;
		}
	}
 
        private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector3)randomPosition, radius);
        } 
}
