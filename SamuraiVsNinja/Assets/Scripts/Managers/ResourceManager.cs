using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
	private Dictionary<int, GameObject[]> prefabs;

    [Space]
	[Header("Characters")]
    public GameObject[] CharacterPrefabs;
    [Space]
    [Header("Collectables")]
    public GameObject[] CollectablePrefabs;
    [Space]
    [Header("Projectiles")]
	public GameObject[] ProjectilePrefabs;
    [Space]
    [Header("UI")]
    public GameObject[] UIPrefabs;
    [Space]
    [Header("Effects")]
    public GameObject[] EffectPrefabs;
    [Space]
    [Header("Others")]
    public GameObject[] OtherPrefabs;

    private GameObject[] networkPrefabs = new GameObject[0];


    private void Awake ()
	{
		InitializePrefabDictionary();
	}
	
	private void InitializePrefabDictionary()
	{
		prefabs = new Dictionary<int, GameObject[]>
		{
			{ 0, CharacterPrefabs },
			{ 1, CollectablePrefabs },
			{ 2, networkPrefabs },
			{ 3, ProjectilePrefabs },
			{ 4, UIPrefabs },
			{ 5, EffectPrefabs },
			{ 6, OtherPrefabs },
		};
	}

	public GameObject GetPrefabByIndex(int prefabKey, int prefabArrayIndex)
	{
        if(prefabs.TryGetValue(prefabKey, out GameObject[] gameObjectPrefabs))
        {
            return gameObjectPrefabs[prefabArrayIndex];
        }

        DebugManager.Instance.DebugMessage(2, "There is not gameobject prefab: " + prefabKey + " !");
		return null;
	}
}
