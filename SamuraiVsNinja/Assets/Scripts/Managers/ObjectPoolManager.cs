using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singelton<ObjectPoolManager>
{
    public List<Transform> ParentContainers = new List<Transform>();

    private Dictionary<string, Stack<GameObject>> poolDictionary = new Dictionary<string, Stack<GameObject>>();

    private void Start()
    {
       
    }

    public GameObject SpawnObject(GameObject prefab, Vector2 position = new Vector2(), Quaternion rotaion = new Quaternion())
    {
        Stack<GameObject> prefabInstances = null;

        if (poolDictionary.TryGetValue(prefab.name, out prefabInstances))
        {
            if (prefabInstances.Count > 0)
            {
                var instance = prefabInstances.Pop();
                instance.transform.SetPositionAndRotation(position, rotaion);
                instance.SetActive(true);
                return instance;
            }
            else
            {
                return CreateNewInstances(prefab, position, rotaion);
            }
        }
        else
        {
            poolDictionary.Add(prefab.name, new Stack<GameObject>());
            return CreateNewInstances(prefab, position, rotaion);
        }
    }

    public void DespawnObject(GameObject instance)
    {
        Stack<GameObject> prefabInstances;

        if (poolDictionary.TryGetValue(instance.name, out prefabInstances))
        {
            prefabInstances.Push(instance);
        }
    }

    private GameObject CreateNewInstances(GameObject prefab, Vector2 position, Quaternion rotation)
    {
        var newInstance = Instantiate(prefab, position, rotation);
        newInstance.name = prefab.name;
        newInstance.transform.SetParent(GetContainer(newInstance.name));
        return newInstance;
    }

    private void PrecreateGameObjects(GameObject prefab, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var newInstance = SpawnObject(prefab);
            newInstance.SetActive(false);
        }
    }

    private Transform GetContainer(string containerName)
    {
        foreach (var container in ParentContainers)
        {
            if (container.name == containerName + "s")
            {
                return container;
            }
        }

        var newContainer = new GameObject(containerName + "s");
        ParentContainers.Add(newContainer.transform);
        newContainer.transform.SetParent(transform);
        return newContainer.transform;
    }
}
