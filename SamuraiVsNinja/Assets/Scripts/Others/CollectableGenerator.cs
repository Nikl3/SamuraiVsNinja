using UnityEngine;

public class CollectableGenerator : MonoBehaviour
{
	#region VARIABLE

	[SerializeField]
	private readonly float radius = 4f;
	[SerializeField]
	private LayerMask collisionLayerMask;
	[SerializeField]
	private Transform onigiris;
	[SerializeField]
	private Transform[] Spawnpoints;
	private GameObject onigiriPrefab;
	private float SpawnIntervall = 0;
	private Vector2 randomPosition;

	#endregion VARIABLE

	private void Start()
	{
		onigiris = transform.parent.Find("Onigiris");
		onigiriPrefab = ResourceManager.Instance.GetPrefabByIndex(1, 0);
	}

	private void Update ()
	{
		SpawnIntervall += Time.deltaTime;

		if (SpawnIntervall > 4f)
		{
			int randomPositionIndex = Random.Range(0, Spawnpoints.Length);
			randomPosition = Spawnpoints[randomPositionIndex].position;

			if (!Physics2D.OverlapCircle(randomPosition, radius, collisionLayerMask))
			{
				var newOnigiri = Instantiate(onigiriPrefab, randomPosition, Quaternion.identity);
				newOnigiri.transform.SetParent(onigiris);
				newOnigiri.name = "Onigiri";
				SpawnIntervall = 0;
			}
			SpawnIntervall = 0f;
		}
	}
 
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(randomPosition, radius);
	} 
}
