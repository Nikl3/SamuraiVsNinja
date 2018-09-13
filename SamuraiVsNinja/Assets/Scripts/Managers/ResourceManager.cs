using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
	private Dictionary<int, GameObject[]> prefabs;

	[Header("Characters")]
	[SerializeField]
	private GameObject[] characterPrefabs = new GameObject[0];
	[Header("Collectables")]
	[SerializeField]
	private GameObject[] collectablePrefabs = new GameObject[0];
	[Header("Network")]
	[SerializeField]
	private GameObject[] networkPrefabs = new GameObject[0];
	[Header("Projectiles")]
	[SerializeField]
	private GameObject[] projectilePrefabs = new GameObject[0];
	[Header("Ui")]
	[SerializeField]
	private GameObject[] uiPrefabs = new GameObject[0];
	[Header("Effects")]
	[SerializeField]
	private GameObject[] effectPrefabs = new GameObject[0];
	[Header("Others")]
	[SerializeField]
	private GameObject[] otherPrefabs = new GameObject[0];

	private void Awake ()
	{
		InitializePrefabDictionary();
	}
	
	private void InitializePrefabDictionary()
	{
		prefabs = new Dictionary<int, GameObject[]>
		{
			{ 0, characterPrefabs },
			{ 1, collectablePrefabs },
			{ 2, networkPrefabs },
			{ 3, projectilePrefabs },
			{ 4, uiPrefabs },
			{ 5, effectPrefabs },
			{ 6, otherPrefabs }
		};
	}

	public GameObject GetPrefabByIndex(int prefabKey, int prefabArrayIndex)
	{
		GameObject[] gameObjectPrefabs;

		if(prefabs.TryGetValue(prefabKey, out gameObjectPrefabs))
		{
			return gameObjectPrefabs[prefabArrayIndex];
		}

		DebugManager.Instance.DebugMessage(2, "There is not gameobject prefab: " + prefabKey + " !");
		return null;
	}
}
