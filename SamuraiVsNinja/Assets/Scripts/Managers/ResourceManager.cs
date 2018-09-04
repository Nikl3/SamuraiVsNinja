using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
	private Dictionary<int, GameObject[]> prefabs;

	[Header("Characters")]
	[SerializeField]
	private GameObject[] characterPrefabs;
	[Header("Collectables")]
	[SerializeField]
	private GameObject[] collectablePrefabs;
	[Header("Network")]
	[SerializeField]
	private GameObject[] networkPrefabs;
	[Header("Projectiles")]
	[SerializeField]
	private GameObject[] projectilePrefabs;
	[Header("Ui")]
	[SerializeField]
	private GameObject[] uiPrefabs;
	[Header("Others")]
	[SerializeField]
	private GameObject[] otherPrefabs;

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
			{ 5, otherPrefabs }
		};
	}

	public GameObject GetPrefabByIndex(int prefabKey, int prefabArrayIndex)
	{
		GameObject[] gameObjectPrefabs;

		if(prefabs.TryGetValue(prefabKey, out gameObjectPrefabs))
		{
			return gameObjectPrefabs[prefabArrayIndex];
		}

		Debug.LogError("There is not gameobject prefab: " + prefabKey + " !");
		return null;
	}
}
