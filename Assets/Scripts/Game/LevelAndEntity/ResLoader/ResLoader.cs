using Game.Core;
using UnityEngine;

namespace Game.LevelAndEntity.ResLoader
{
    public class ResLoader : ManagerBase, IResLoader
    {
        public bool LoadRes(string path, out GameObject obj)
        {
            obj = new GameObject();
            return false;
        }

        public bool UnloadRes(string path)
        {
            return true;
        }
    }
}