using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MGM.Core
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class LookAtSystem : JobComponentSystem
    {
        private EntityQuery m_Targets;

        protected override void OnCreate()
        {
            m_Targets = GetEntityQuery(typeof(IsTargeted), ComponentType.ReadOnly<LocalToWorld>());
        }
        [BurstCompile]
        struct LookAtJob : IJobForEachWithEntity<Target, LocalToWorld, Rotation>
        {

            [ReadOnly][DeallocateOnJobCompletion] public NativeArray<Entity> targetEntities;
            [ReadOnly][DeallocateOnJobCompletion] public NativeArray<LocalToWorld> targetPositions;

            public void Execute(Entity entity, int index, [ReadOnly] ref Target target, [ReadOnly] ref LocalToWorld localToWorld,
                ref Rotation rotation)
            {

                for (int i=0;i< targetEntities.Length;i++)
                {
                    if (targetEntities[i].Equals(target.Entity)) {
                        var lookAtPoint = targetPositions[i].Position - localToWorld.Position;
                        lookAtPoint.y = 0;
                        rotation.Value = quaternion.LookRotationSafe(lookAtPoint, localToWorld.Up);
                    }
                }
                
            }
                        
        }

       
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {


            LookAtJob laj = new LookAtJob()
            {
                targetEntities = m_Targets.ToEntityArray(Allocator.TempJob),
                targetPositions = m_Targets.ToComponentDataArray<LocalToWorld>(Allocator.TempJob)
            };
            return laj.Schedule(this,inputDeps);
        }
    }
}


