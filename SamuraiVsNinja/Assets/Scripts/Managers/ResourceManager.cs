using UnityEngine;

public class ResourceManager : SingeltonPersistant<ResourceManager>
{
	[SerializeField]
	private GameObject[] gameObjectPrefabs;

	protected override void Awake ()
	{
		base.Awake();
		gameObjectPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs/") as GameObject[];
	}
	
	public GameObject GetPrefabByName(string prefabName)
	{
		foreach (var prefab in gameObjectPrefabs)
		{
			if(prefab.name == prefabName)
			{
				return prefab;
			}
		}

		Debug.LogError("There is not gameobject prefab: " + prefabName + " !");
		return null;
	}
}
