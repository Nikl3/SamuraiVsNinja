using System.Collections.Generic;
using UnityEngine;



public class ObjectPoolManager : Singelton<ObjectPoolManager>
{
    private Dictionary<GameObject, Stack<GameObject>> poolDictionary = new Dictionary<GameObject, Stack<GameObject>>();

    public GameObject SpawnObject(GameObject prefabInstance)
    {
        Stack<GameObject> prefabInstances = null;

        poolDictionary.TryGetValue(prefabInstance, out prefabInstances);

        if(prefabInstances != null && prefabInstances.Count > 0)
        {
            return prefabInstances.Pop();
        }
        else
        {
            return CreateNewPrefabInstance(prefabInstance);
        }
    }

    private GameObject CreateNewPrefabInstance(GameObject prefabInstance)
    {
        return Instantiate(prefabInstance);
    }
}
