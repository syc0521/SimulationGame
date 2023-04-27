using Game.Audio;
using Game.Core;
using Game.Data;
using Game.GamePlaySystem;
using Game.Input;
using Game.LevelAndEntity.ResLoader;
using Game.UI;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

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