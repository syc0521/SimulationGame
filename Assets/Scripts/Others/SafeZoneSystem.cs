using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    public partial struct SafeZoneSystem : ISystem
    {
        public ComponentLookup<Shooting> turretActiveFromEntity;
    
        public void OnCreate(ref SystemState state)
        {
            turretActiveFromEntity = state.GetComponentLookup<Shooting>();
        }

        public void OnDestroy(ref SystemState state)
        {
        
        }

        public void OnUpdate(ref SystemState state)
        {
            float radius = 20;
            const float renderStep = 20;

            for (float angle = 0; angle < 360; angle += renderStep)
            {
                var a = float3.zero;
                var b = float3.zero;
                math.sincos(math.radians(angle), out a.x, out a.z);
                math.sincos(math.radians(angle + renderStep), out b.x, out b.z);
                Debug.DrawLine(a * radius, b * radius);
            }
        
            turretActiveFromEntity.Update(ref state);
            var safeZoneJob = new SafeZoneJob()
            {
                turretActiveFromEntity = turretActiveFromEntity,
                squaredRadius = radius * radius
            };
            //safeZoneJob.ScheduleParallel<SafeZoneJob>();
        }
    }

    [WithAll(typeof(Turret))]
    [BurstCompile]
    public partial struct SafeZoneJob : IJobEntity
    {
        [NativeDisableParallelForRestriction] public ComponentLookup<Shooting> turretActiveFromEntity;
        public float squaredRadius;

        void Execute(Entity entity, TransformAspect transform)
        {
            turretActiveFromEntity.SetComponentEnabled(entity, math.lengthsq(transform.Position) > squaredRadius);
        }
    }
}