using Game.Data;
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
    
    [GMAttr(type = "建筑", name = "建筑全解锁", priority = 2)]
    public class UnlockAllBuildingCommand : ICommand
    {
        public void Run()
        {
            var buildingData = ConfigTable.Instance.GetAllBuildingData();
            foreach (var data in buildingData)
            {
                BuildingManager.Instance.UnlockBuilding(data.Buildingid);
            }
        }
    }
}