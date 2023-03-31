using System;

namespace Game.Core
{
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 在对象池里查找并返回相关对象
        /// </summary>
        /// <returns></returns>
        public T GetItem(Predicate<T> condition);
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <returns></returns>
        public void RecycleItem(T poolObject);
    }
}