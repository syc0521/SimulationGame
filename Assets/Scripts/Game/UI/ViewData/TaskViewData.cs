using System.Collections.Generic;
using Game.Data;

namespace Game.UI.ViewData
{
    public record TaskViewData
    {
        public string name;
        public List<RewardData> reward;
        public string content;
        public TaskState state;
        public int[] targetID;
        public int[] currentNum;
        public int[] targetNum;
        public TaskType type;
    }
}