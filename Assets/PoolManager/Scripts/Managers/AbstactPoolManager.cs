using System.Collections.Generic;
using System;

namespace ManagerPooling
{
    public static class AbstactPoolManager<T>
    {
        public static Dictionary<string, Pool<T>> AllPools = new Dictionary<string, Pool<T>>();

        public static void CreateNewPool(string idGroup, List<T> objectsForPool)
        {
            var newPool = new Pool<T>(objectsForPool);
            AllPools.Add(idGroup, newPool);
        }
        public static T GetFromPool(string idGroup,T itemForPool, Func<T> createObjectFunc)
        {
            if (AllPools.ContainsKey(idGroup))
            {
                var objectFromPool = AllPools[idGroup].GetFromPool();
                return objectFromPool;
            }
            else
            {
                var newPool = new Pool<T>(createObjectFunc,1);
                AllPools.Add(idGroup, newPool);
                return AllPools[idGroup].GetFromPool();
            }
        }

        public static void BackToPool(string idGroup, T item)
        {
            if (AllPools.ContainsKey(idGroup) && AllPools[idGroup].ObjectsInPool.ContainsKey(item))
            {
                AllPools[idGroup].ObjectsInPool[item].Free = true;
            }
        }
    }
}
