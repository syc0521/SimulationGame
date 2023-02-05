using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Core;

namespace Game.GamePlaySystem
{
    public class GamePlaySystemManager : ManagerBase, IGamePlaySystemManager
    {
        private List<IGamePlaySystem> _systems = new();

        public override void OnAwake()
        {
            var typeGamePlayModule = typeof(IGamePlaySystem);
            var result = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeGamePlayModule.IsAssignableFrom(t));
            foreach( var type in result )
            {
                if (type == typeGamePlayModule || type.IsAbstract)
                {
                    continue;
                }
                var constructorInfo = type.GetConstructor(new Type[] {});
                _systems.Add((IGamePlaySystem)constructorInfo?.Invoke(new object[] {}));
            }
            
            foreach (var system in _systems)
            {
                system.OnAwake();
            }
        }

        public override void OnStart()
        {
            foreach (var system in _systems)
            {
                system.OnStart();
            }
        }

        public override void OnUpdate()
        {
            foreach (var system in _systems)
            {
                system.OnUpdate();
            }
        }

        public override void OnDestroyed()
        {
            foreach (var system in _systems)
            {
                system.OnDestroyed();
            }
        }
    }
}