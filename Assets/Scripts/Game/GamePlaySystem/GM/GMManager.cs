using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Data.GM;

namespace Game.GamePlaySystem.GM
{
    public struct Command
    {
        public string name;
        public Type type;
        public int priority;
    }
    
    public class GMManager : GamePlaySystemBase<GMManager>
    {
        private Dictionary<string, List<Command>> _commandDic;

        public override void OnAwake()
        {
            base.OnAwake();
            CollectGMCommands();
        }

        public override void OnDestroyed()
        {
            _commandDic = null;
            base.OnDestroyed();
        }

        private void CollectGMCommands()
        {
            _commandDic = new();
            var type = typeof(ICommand);
            var result = Assembly.GetExecutingAssembly().GetTypes().Where(t => type.IsAssignableFrom(t));

            foreach (var commandType in result)
            {
                foreach (var attributes in commandType.GetCustomAttributes(false))
                {
                    var gmAttr = (GMAttr)attributes;
                    if (!_commandDic.ContainsKey(gmAttr.type))
                    {
                        _commandDic[gmAttr.type] = new List<Command>();
                    }
                    _commandDic[gmAttr.type].Add(new Command
                    {
                        name = gmAttr.name,
                        priority = gmAttr.priority,
                        type = commandType
                    });
                }
            }

            foreach (var commandList in _commandDic.Values)
            {
                commandList.Sort((a,b) => b.priority - a.priority);
            }
        }

        public List<string> GetGMCategory() => _commandDic.Keys.ToList();

        public List<string> GetGMNames(string type)
        {
            return _commandDic[type].Select(item => item.name).ToList();
        }

        public Type GetGMType(string type, int index)
        {
            return _commandDic[type][index].type;
        }
    }
}