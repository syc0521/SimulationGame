using Game.Data.Common;

namespace Game.UI.ViewData
{
    public record BuildingViewData
    {
        public string name;
        public string description;
        public CurrencyType[] currencyType;
        public int[] currencyCount;
        public bool isUnlock;
    }
}