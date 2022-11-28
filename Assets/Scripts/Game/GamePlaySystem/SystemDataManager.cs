using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public class SystemDataManager : GamePlaySystemBase<SystemDataManager>
    {
        public ValueChangedEvent<int> People { get; private set; }
        public override void OnStart()
        {
            People = new(OnPeopleChange);
        }

        public override void OnUpdate()
        {
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<Config>();
            var singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            People.Value = singletonQuery.GetSingleton<Config>().people;
            Debug.Log(People);
        }

        private void OnPeopleChange(int people)
        {
            
        }
    }
}