using Game.Core;
using Game.LevelAndEntity.ResLoader;
using UnityEngine.UI;

namespace Game.UI.Component
{
    public class CustomImage : Image
    {
        public void SetIcon(AtlasSpriteID atlasSpriteID)
        {
            Managers.Get<IResLoader>().LoadRes(ResEnum.sprite, GetAssetPath(atlasSpriteID), handle =>
            {
                sprite = handle.Result;
            });
        }

        private string GetAssetPath(AtlasSpriteID atlasSpriteID)
        {
            switch (atlasSpriteID.atlas)
            {
                case AtlasEnum.Task:
                    return $"task/{atlasSpriteID.resName}";
            }
            return null;
        }
    }
}