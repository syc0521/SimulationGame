using Game.Data;
using Game.Data.TableData;

namespace Game.GamePlaySystem.Task
{
    public static class TaskDecorator
    {
        public static TaskState GetTaskState(this TaskData taskData)
        {
            return TaskManager.Instance.GetTaskState(taskData.Taskid);
        }

        public static string GetTaskTargetName(this TaskType type, int id)
        {
            return type switch
            {
                TaskType.AddBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.MoveBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.RotateBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.UpgradeBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.GetCurrency => ConfigTable.Instance.GetCurrencyData(id).Name,
                TaskType.GetBagItem => ConfigTable.Instance.GetBagItemData(id).Name,
                TaskType.CountBuilding => GetBuildingTypeDesc(id),
                TaskType.GetEvaluateScore => "城市面貌",
                TaskType.People => "人口",
                TaskType.BuyItem => "购买",
                TaskType.BuyDailyItem => "购买",
                TaskType.SellItem => "出售",
                TaskType.SellToGetCurrency => ConfigTable.Instance.GetCurrencyData(id).Name,
                _ => string.Empty
            };
        }

        private static string GetBuildingTypeDesc(int type)
        {
            return type switch
            {
                -1 => "建筑",
                0 => "住宅",
                1 => "生产建筑",
                2 => "装饰",
                3 => "地标",
                _ => string.Empty,
            };
        }
    }
}