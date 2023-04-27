using System;
using Game.Data.Common;
using Game.Data.GM;
using Game.GamePlaySystem.Currency;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "货币", name = "添加货币", priority = 1)]
    public class AddCurrencyCommand : ICommand
    {
        public int currencyId;
        public int count;
        
        public void Run()
        {
            CurrencyManager.Instance.AddCurrency((CurrencyType)currencyId, count);
        }
    }
    
    [GMAttr(type = "货币", name = "添加各种货币1w", priority = 2)]
    public class AddManyCurrencyCommand : ICommand
    {
        public void Run()
        {
            foreach (var item in Enum.GetValues(typeof(CurrencyType)))
            {
                CurrencyManager.Instance.AddCurrency((CurrencyType)item, 10000);
            }
        }
    }
}