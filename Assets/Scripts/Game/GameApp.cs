using Game.Core;
using Game.Data;
using Game.Input;
using Game.LevelAndEntity.ResLoader;
using Game.UI;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public class GameApp : Singleton<GameApp>
    {
        // todo 游戏成型之后将其作为游戏唯一入口
        // 目前只是起着管理器的作用

        protected override void Awake()
        {
            //初始化管理器
            base.Awake();
            Application.targetFrameRate = 90;
            
            Managers.Register<ISaveDataManager, SaveDataManager>();
            Managers.Register<IResLoader, ResLoader>();
            Managers.Register<IInputManager, InputManager>();
            Managers.Register<IUIManager, UIManager>();

            //开始管理器逻辑
            Managers.Start<ISaveDataManager>();
            Managers.Start<IResLoader>();
            Managers.Start<IInputManager>();
            Managers.Start<IUIManager>();
        }
        

        private void OnDestroy()
        {
            Managers.Unregister<IUIManager>();
            Managers.Unregister<IInputManager>();
            Managers.Unregister<IResLoader>();
            Managers.Unregister<ISaveDataManager>();
        }

        private void Update()
        {
            Managers.Update<ISaveDataManager>();
            Managers.Update<IResLoader>();
            Managers.Update<IInputManager>();
            Managers.Update<IUIManager>();
        }
    }
}