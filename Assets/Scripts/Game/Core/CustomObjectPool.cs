using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class PoolObject
    {
        public int reference;
    }

    public abstract class CustomObjectPool<T> : IObjectPool<T> where T : PoolObject, new()
    {
        private List<T> pool = new();
        protected int ObjectCount => pool.Count;

        protected T CreateItem()
        {
            var poolObject = new T();
            OnCreateItem(poolObject);
            poolObject.reference++;
            pool?.Add(poolObject);

            return poolObject;
        }

        public T GetItem(Predicate<T> condition)
        {
            var poolObject = pool?.Find(condition);
            if (poolObject != null)
            {
                poolObject.reference++;
            }

            return poolObject;
        }

        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected T FindItem(Predicate<T> condition)
        {
            return pool?.Find(condition);
        }

        public void RecycleItem(Predicate<T> condition)
        {
            var poolObject = pool?.Find(condition);
            if (poolObject == null) return;
            RecycleItem(poolObject);
        }

        public void RecycleItem(T poolObject)
        {
            if (poolObject == null) return;
            poolObject.reference--;
            OnRecycleItem(poolObject);
            if (poolObject.reference <= 0)
            {
                DestroyItem(poolObject);
            }
        }

        protected void DestroyItem(T poolObject)
        {
            pool?.Remove(poolObject);
            OnDestroyItem(poolObject);
        }
        
        protected virtual void OnCreateItem(T poolObject){}
        protected virtual void OnRecycleItem(T poolObject){}
        protected virtual void OnDestroyItem(T poolObject){}
    }
}