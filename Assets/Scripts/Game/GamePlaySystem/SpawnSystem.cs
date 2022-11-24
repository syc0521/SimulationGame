using System;
using System.Runtime.CompilerServices;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public struct BuildEvent : IEvent
{
    public BuildState buildState;
    public int buildingType;
}

namespace Game.GamePlaySystem
{
    [Obsolete]
    public class SpawnSystem : MonoBehaviour
    {
        public Camera mainCamera;
        private World world;
        private RaycastInput _raycastInput;
        private bool hasRaycastInput;
        private GameControl control;
        private int buildingType = 0;
        private Grid<int> Grid = new(15, 15, -1);

        private void Awake()
        {
            control = new();
        }

        private void OnEnable()
        {
            control?.Enable();
        }

        private void OnDisable()
        {
            control?.Disable();
        }

        private void Start()
        {
            world = World.DefaultGameObjectInjectionWorld;
            control.GamePlay.RightClick.performed += RemoveBuilding;
        }

        private void OnDestroy()
        {
            Grid.Dispose();
        }

        private void RemoveBuilding(InputAction.CallbackContext ctx)
        {
            var collisionWorld = GetCollisionWorld();
            var raycastInput = GetOrCreateRaycastInput();

            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<Building>(entity))
                {
                    entityManager.AddComponent<RemoveBuilding>(entity);
                }
            }
        }

        private RaycastInput GetOrCreateRaycastInput()
        {
            if (hasRaycastInput)
            {
                return _raycastInput;
            }
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
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

        private CollisionWorld GetCollisionWorld()
        {
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();
            var singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singletonQuery.Dispose();
            return collisionWorld;
        }

    }
}
