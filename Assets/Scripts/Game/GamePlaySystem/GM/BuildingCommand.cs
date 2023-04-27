using Game.Data.GM;
using Game.GamePlaySystem.Build;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "建筑", name = "解锁特定建筑", priority = 1)]
    public class UnlockBuildingCommand : ICommand
    {
        public int buildingId;
        
        public void Run()
        {
            BuildingManager.Instance.UnlockBuilding(buildingId);
        }
    }
}