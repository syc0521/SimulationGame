using Game.Data;

namespace Game.UI.ViewData
{
    public record TaskViewData
    {
        public string name;
        public RewardData reward;
        public string content;
        public TaskState state;
        public int[] targetID;
        public int[] currentNum;
        public int[] targetNum;
    }
}