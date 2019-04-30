using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
namespace PONG {
    public class ScoringSystem : JobComponentSystem
    {
        struct UpdateScoreJob : IJobForEach<Score, Translation, PhysicsVelocity>
        {
            public void Execute(ref Score score, ref Translation position, ref PhysicsVelocity physicsVelocity)
            {
          
                if (position.Value.x > 8)
                {
                    score.Player1++;
                    physicsVelocity.Linear.z = position.Value.z;
                    physicsVelocity.Linear.x = -5;
                    position.Value = new float3();
                }
                if (position.Value.x < -8)
                {
                    score.Player2++;
                    physicsVelocity.Linear.z = position.Value.z;
                    physicsVelocity.Linear.x = 5;
                    position.Value = new float3();
                }

  

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new UpdateScoreJob().Schedule(this, inputDeps);
        }
    }
}
