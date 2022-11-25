using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class MonoApp : Singleton<MonoApp>
    {
        private List<Delegate> updateAction = new();
        private void Update()
        {
            foreach (var func in updateAction)
            {
                func?.DynamicInvoke();
            }
        }

        public void AddUpdateFunc(Action func)
        {
            updateAction.Add(func);
        }

        public void RemoveUpdateFunc(Action func)
        {
            updateAction.Remove(func);
        }
    }
}