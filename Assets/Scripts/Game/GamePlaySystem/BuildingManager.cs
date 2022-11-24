using Game.Core;
using Game.Data;
using Game.GamePlaySystem.GameState;
using Game.GamePlaySystem.StateMachine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public struct PlaceEvent : IEvent
    {
        public Vector3 pos;
    }
    
    public enum BuildState
    {
        Normal, Select, Construct, Place, Rotate, Delete
    }
    public class BuildingManager : GamePlaySystemBase<BuildingManager>
    {
        public Camera mainCamera;
        
        private RaycastInput _raycastInput;
        private bool hasRaycastInput;
        private Grid<int> grid = new(15, 15, -1);
        private StateMachine.StateMachine buildStateMachine;

        public override void OnStart()
        {
            buildStateMachine = new(new IState[]
            {
                new NormalState(),
                new SelectState(),
                new AddBuildingState(),
            });
        }
        
        public void SelectBuilding()
        {
            buildStateMachine.ChangeState<SelectState>();
        }
        
        /// <summary>
        /// 添加建筑，生成对应UI
        /// </summary>
        public void AddBuilding(int type)
        {
            buildStateMachine.ChangeState<AddBuildingState>(type);
        }
        
        public void ConstructBuilding()
        {
            buildStateMachine.ChangeState<NormalState>(true);
        }

        public void DeleteTempBuilding()
        {
            buildStateMachine.ChangeState<NormalState>(false);
        }

        /// <summary>
        /// 删除建筑物
        /// </summary>
        public void RemoveBuilding()
        {
            
        }

        public void RotateBuilding()
        {
            
        }
        
        public RaycastInput GetOrCreateRaycastInput(float3 pos)
        {
            if (hasRaycastInput)
            {
                return _raycastInput;
            }
            var ray = mainCamera.ScreenPointToRay(pos);
            _raycastInput = new RaycastInput
            {
                Start = ray.origin,
                End = ray.GetPoint(50),
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                }
            };
            return _raycastInput;
        }

        public CollisionWorld GetCollisionWorld()
        {
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();
            var singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singletonQuery.Dispose();
            return collisionWorld;
        }

        public Grid<int> GetGrid() => grid;
    }
}