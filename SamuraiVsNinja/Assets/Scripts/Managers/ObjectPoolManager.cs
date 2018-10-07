using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singelton<ObjectPoolManager>
{
    private List<Transform> ParentContainers = new List<Transform>();
    private Dictionary<string, Stack<GameObject>> poolDictionary = new Dictionary<string, Stack<GameObject>>();

    private GameObject CreateNewInstances(GameObject prefab, Vector2 position, Quaternion rotation)
    {
        DebugManager.Instance.DebugMessage(2, prefab.name + " created!");
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

    public GameObject SpawnObject(GameObject prefab, Vector2 position = new Vector2(), Quaternion rotation = new Quaternion(), bool isActive = true)
    {
        Stack<GameObject> prefabInstances = null;

        if (poolDictionary.TryGetValue(prefab.name, out prefabInstances))
        {
            if (prefabInstances.Count > 0)
            {
                var instance = prefabInstances.Pop();

                DebugManager.Instance.DebugMessage(1, "Re-used instance: " + instance.name);

                instance.transform.SetPositionAndRotation(position, rotation);
                instance.SetActive(true);
                return instance;
            }
            else
            {
                return CreateNewInstances(prefab, position, rotation);
            }
        }
        else
        {
            DebugManager.Instance.DebugMessage(2,"Created a new empty stack and instance");
            poolDictionary.Add(prefab.name, new Stack<GameObject>());
            return CreateNewInstances(prefab, position, rotation);
        }
    }
    public void DespawnObject(GameObject instance)
    {
        instance.SetActive(false);

        Stack<GameObject> prefabInstances;

        if (poolDictionary.TryGetValue(instance.name, out prefabInstances))
        {
            DebugManager.Instance.DebugMessage(1, instance.name + " pushed to correct stack: " + instance.name);
            prefabInstances.Push(instance);
        }
        else
        {
            DebugManager.Instance.DebugMessage(2, "Created a new empty stack: " + instance.name);
            poolDictionary.Add(instance.name, new Stack<GameObject>());
            prefabInstances = poolDictionary[instance.name];
            prefabInstances.Push(instance);
        }
    }
}
