using System;
using Game.Core;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.LevelAndEntity.ResLoader
{
    public interface IResLoader : IManager
    {
        void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<GameObject>> callback);
        void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<Texture2D>> callback);
        bool UnloadRes(ResEnum type, string path);
    }
}