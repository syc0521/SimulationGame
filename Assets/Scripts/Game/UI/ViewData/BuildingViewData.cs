using Game.Data.Common;

namespace Game.UI.ViewData
{
    public record BuildingViewData
    {
        public string name;
        public string description;
        public int[] itemType;
        public int[] itemCount;
        public bool isUnlock;
    }
}