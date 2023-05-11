using Game.Core;
using Game.LevelAndEntity.ResLoader;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Game.UI.Component
{
    public class CustomImage : Image
    {
        private AsyncOperationHandle<Texture2D> _handle;

        protected override void OnDestroy()
        {
            if (sprite != null && string.IsNullOrEmpty(sprite.name))
            {
                Destroy(sprite);
            }
            base.OnDestroy();
        }

        public void OnDestroyed()
        {
            if (_handle.IsValid())
            {
                Managers.Get<IResLoader>()?.UnloadRes(_handle);
            }
        }

        public void SetIcon(AtlasSpriteID atlasSpriteID)
        {
            LoadSprite(ResEnum.Sprite, GetAssetPath(atlasSpriteID));
        }

        public void SetPicture(string picName)
        {
            LoadSprite(ResEnum.Picture, picName);
        }

        private void LoadSprite(ResEnum resEnum, string spriteName)
        {
            Managers.Get<IResLoader>().LoadRes(resEnum, spriteName, handle =>
            {
                if (handle.Result != null)
                {
                    _handle = handle;
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