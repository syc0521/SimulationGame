using Game.LevelAndEntity.Component;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Game.LevelAndEntity.System
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class CameraSystem : SystemBase
    {
        private Entity target;
        private Random random;
        private EntityQuery tanksQuery;
        protected override void OnCreate()
        {
            random = Random.CreateFromIndex(1234);
            tanksQuery = GetEntityQuery(typeof(Tank));
            RequireForUpdate(tanksQuery);
        }

        protected override void OnUpdate()
        {
            /*if (target == Entity.Null || Input.GetKeyDown(KeyCode.Space))
        {
            var tanks = tanksQuery.ToEntityArray(Allocator.Temp);
            target = tanks[random.NextInt(tanks.Length)];
        }

        var cameraTransform = CameraSingleton.Instance.transform;
        var tankTransform = GetComponent<LocalToWorld>(target);
        cameraTransform.position = tankTransform.Position + 10.0f * tankTransform.Forward * new float3(1, 1, 0);
        cameraTransform.LookAt(tankTransform.Position, new float3(0,1,0));*/
        }
    }
}