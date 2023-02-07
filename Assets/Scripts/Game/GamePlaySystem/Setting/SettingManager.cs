using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.GamePlaySystem.Setting
{
    public class SettingManager : GamePlaySystemBase<SettingManager>
    {
        public override void OnUpdate()
        {
            // 调试用
            if (UnityEngine.Input.GetKey(KeyCode.F9))
            {
                ResetSaveData();
            }
        }

        public void ResetSaveData()
        {
            Managers.Get<ISaveDataManager>().ResetSaveData();
        }
    }
}