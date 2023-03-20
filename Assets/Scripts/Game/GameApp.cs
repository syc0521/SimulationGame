using System;
using System.Diagnostics;
using Game.Audio;
using Game.Core;
using Game.Data;
using Game.GamePlaySystem;
using Game.GamePlaySystem.BurstUtil;
using Game.Input;
using Game.LevelAndEntity.ResLoader;
using Game.UI;
using MessagePack;
using MessagePack.Resolvers;
using Unity.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Grid = Game.Data.Grid;

namespace Game
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public class GameApp : Singleton<GameApp>
    {
        protected override void Awake()
        {
            //初始化管理器
            base.Awake();
            Application.targetFrameRate = 90;
            
            Managers.Register<ISaveDataManager, SaveDataManager>();
            Managers.Register<IResLoader, ResLoader>();
            Managers.Register<IInputManager, InputManager>();
            Managers.Register<IAudioManager, AudioManager>();
            Managers.Register<IGamePlaySystemManager, GamePlaySystemManager>();
            Managers.Register<IUIManager, UIManager>();

            //开始管理器逻辑
            Managers.Start<ISaveDataManager>();
            Managers.Start<IResLoader>();
            Managers.Start<IInputManager>();
            Managers.Start<IAudioManager>();
            Managers.Start<IGamePlaySystemManager>();
            Managers.Start<IUIManager>();
        }

        private void Start()
        {
            /*Stopwatch sw = new Stopwatch();
            var testArr = new int[100, 100];
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                BuildingUtils.BurstTest_UnBurst(ref testArr, 100);
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Debug.Log($"UnBurst {ts}");
            
            sw = new Stopwatch();
            var testNativeArr = new NativeArray<int>(10000, Allocator.TempJob);
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                BuildingUtils.BurstTest_Burst(ref testNativeArr, 100);
            }
            sw.Stop();
            ts = sw.Elapsed;
            Debug.Log($"Burst {ts}");*/
        }


        private void OnDestroy()
        {
            Managers.Unregister<IUIManager>();
            Managers.Unregister<IGamePlaySystemManager>();
            Managers.Unregister<IAudioManager>();
            Managers.Unregister<IInputManager>();
            Managers.Unregister<IResLoader>();
            Managers.Unregister<ISaveDataManager>();
        }

        private void Update()
        {
            Managers.Update<ISaveDataManager>();
            Managers.Update<IResLoader>();
            Managers.Update<IInputManager>();
            Managers.Update<IGamePlaySystemManager>();
            Managers.Update<IUIManager>();
        }
        
        static bool serializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (!serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    MessagePack.Resolvers.GeneratedResolver.Instance,
                    MessagePack.Resolvers.StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                serializerRegistered = true;
            }
        }

#if UNITY_EDITOR


        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            Initialize();
        }

#endif
    }
}