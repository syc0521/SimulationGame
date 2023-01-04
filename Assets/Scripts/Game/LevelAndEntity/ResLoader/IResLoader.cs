using Game.Core;
using UnityEngine;

namespace Game.LevelAndEntity.ResLoader
{
    public interface IResLoader : IManager
    {
        bool LoadRes(string path, out GameObject obj);
        bool UnloadRes(string path);
    }
}