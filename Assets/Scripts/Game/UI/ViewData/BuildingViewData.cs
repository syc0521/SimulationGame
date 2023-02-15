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
        public BuildingType buildingType;
    }

    public enum BuildingType
    {
        All = 0, 
        House = 1,
        Producer = 2,
        Decorate = 3,
        Landmark = 4,
    }
}