using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class ObjectPoolManager : Singelton<ObjectPoolManager>
    {
        #region VARIABLES
        
        private Type typeKey;

        private Dictionary<Type, MonoBehaviour> gameAssetsDictionary = new Dictionary<Type, MonoBehaviour>();
        private Dictionary<Type, Stack<MonoBehaviour>> poolDictionary = new Dictionary<Type, Stack<MonoBehaviour>>();

        private readonly string assetsPath = string.Empty;

        #endregion VARIABLES

        #region PROPERTIES

        public List<Type> Primitives
        {
            get;
            private set;
        } = new List<Type>();

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Initialize();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void Initialize()
        {
            var assets = Resources.LoadAll<MonoBehaviour>(assetsPath);

            Type type;

            for(int i = 0; i < assets.Length; i++)
            {
                type = assets[i].GetType();
                if(gameAssetsDictionary.ContainsKey(type))
                {
                    continue;
                }

                gameAssetsDictionary.Add(type, assets[i]);
                poolDictionary.Add(assets[i].GetType(), new Stack<MonoBehaviour>());
            }
        }

        private T CreateInstance<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            typeKey = typeof(T);
            
            if(gameAssetsDictionary.ContainsKey(typeKey))
            {
                var newInstance = Instantiate(gameAssetsDictionary[typeKey]) as T;
                newInstance.transform.SetPositionAndRotation(position, rotation);
                newInstance.name = typeKey.Name;
                return newInstance;
            }

            return null;
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            typeKey = typeof(T);

            if(poolDictionary.ContainsKey(typeKey))
            {
                if(poolDictionary[typeKey].Count == 0)
                {
                    return CreateInstance<T>(position, rotation);
                }
                else
                {
                    var instnace = poolDictionary[typeKey].Pop();
                    instnace.transform.SetPositionAndRotation(position, rotation);
                    instnace.gameObject.SetActive(true);
                    return instnace as T;
                }
            }
            else
            {
                return CreateInstance<T>(position, rotation);
            }
        }

        public void Despawn(MonoBehaviour instnace)
        {
            typeKey = instnace.GetType();

            if(poolDictionary.ContainsKey(typeKey))
            {
                if(poolDictionary[typeKey] == null)
                {
                    poolDictionary.Add(typeKey, new Stack<MonoBehaviour>());
                }

                instnace.gameObject.SetActive(false);
                poolDictionary[typeKey].Push(instnace);
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}