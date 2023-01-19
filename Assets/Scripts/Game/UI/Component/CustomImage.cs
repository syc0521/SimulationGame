using Game.Core;
using Game.LevelAndEntity.ResLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Component
{
    public class CustomImage : Image
    {
        public void SetIcon(AtlasSpriteID atlasSpriteID)
        {
            Managers.Get<IResLoader>().LoadRes(ResEnum.sprite, GetAssetPath(atlasSpriteID), handle =>
            {
                if (handle.Result != null)
                {
                    var s = Sprite.Create(handle.Result, new Rect(0, 0, handle.Result.width, handle.Result.height), Vector2.zero);
                    if (this != null)
                    {
                        sprite = s;
                    }
                }
            });
        }

        private string GetAssetPath(AtlasSpriteID atlasSpriteID)
        {
            return $"{atlasSpriteID.atlas.ToString().ToLower()}/{atlasSpriteID.resName}";
        }
    }
}