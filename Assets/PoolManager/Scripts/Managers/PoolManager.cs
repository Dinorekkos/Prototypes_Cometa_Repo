using UnityEngine;
using System.Collections.Generic;

namespace ManagerPooling
{
    public class PoolManager : MonoBehaviour
    {
        public static Dictionary<string, Pool<GameObject>> AllPools = new Dictionary<string, Pool<GameObject>>();
    
        public enum SceneType
        {
            OnceLoad, DontDestroy
        }
        public SceneType TypeLoad;

        private PoolManager instance = null;

        private void Awake()
        {
            SetTypeManager();
        }

        private void SetTypeManager()
        {
            switch (TypeLoad)
            {
                case SceneType.OnceLoad: { break; }
                case SceneType.DontDestroy:
                    {
                        if (!instance)
                            DontDestroyOnLoad(this);
                        else Destroy(this);
                        break;
                    }
            }
        }

        public static void PoolInstaller(GameObject prefab, int size, string idGroup)
        {
            var rootTransform = new GameObject();
            rootTransform.name = prefab.name + " Pool";
            var pool = new Pool<GameObject>(() => { return InstantiateObject(prefab, rootTransform); }, size);
            AllPools.Add(idGroup, pool);
        }

        public static void SpawnObject(GameObject prefab,string idGroup)
        {
            if (AllPools.ContainsKey(idGroup))
            {
                var objectFromPool = AllPools[idGroup].GetFromPool();
                objectFromPool.SetActive(true);
            }
            else
            {
                PoolInstaller(prefab, 1, idGroup);
            }
        }

        public static void BackToPool(GameObject item,string idGroup)
        {
            if (AllPools.ContainsKey(idGroup))
            {
                item.SetActive(false);
                AllPools[idGroup].BackToPool(item);
            }
        }

        private static GameObject InstantiateObject(GameObject prefab, GameObject rootTransform)
        {
            var newObject = Instantiate(prefab) as GameObject;
            newObject.transform.SetParent(rootTransform.transform);
            newObject.SetActive(false);
            return newObject;
        }
    }
}
