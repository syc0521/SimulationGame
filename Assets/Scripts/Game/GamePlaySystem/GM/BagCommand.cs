using Game.Data.GM;
using Game.GamePlaySystem.Backpack;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "背包", name = "添加背包物品", priority = 1)]
    public class AddBagItemCommand : ICommand
    {
        public int itemId;
        public int count;
        
        public void Run()
        {
            BackpackManager.Instance.AddBackpackCount(itemId, count);
        }
    }
}