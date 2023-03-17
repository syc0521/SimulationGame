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

        public static string GetTaskTargetName(TaskType type, int id)
        {
            return type switch
            {
                TaskType.AddBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.MoveBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.RotateBuilding => ConfigTable.Instance.GetBuildingData(id).Name,
                TaskType.GetCurrency => ConfigTable.Instance.GetCurrencyData(id).Name,
                TaskType.GetBagItem => ConfigTable.Instance.GetBagItemData(id).Name,
                _ => string.Empty
            };
        }
    }
}