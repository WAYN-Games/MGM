using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
namespace Wayn.Mgm.Effects
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithECB<E> : ConsumerSystem<EffectConsumerSystemWithECB<E>.ConsumerJob, E>
    where E : struct, IEffectApplierWithECB, IEffect
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public int JobIndex;
            public EntityCommandBuffer.Concurrent ECB;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            [ReadOnly] public BufferFromEntity<Child> ChildFromEntity;


            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {
                    // Apply the effect to the component data.
                    RecursiveChildsApply(effect.Other, effect);
                }
            }

            void RecursiveChildsApply(Entity entity, E effect)
            {
                if (ChildFromEntity.Exists(entity) && effect.ApplyRecursivelyToChildren)
                {
                    var children = ChildFromEntity[entity];
                    for (int i = 0; i < children.Length; i++)
                    {
                        RecursiveChildsApply(children[i].Value, effect);
                    }
                }
                effect.Apply(in JobIndex, entity, ref ECB);
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                JobIndex = GetType().GetHashCode(),
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue,
                ChildFromEntity = GetBufferFromEntity<Child>(true)
            };
        }
    }


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithDeltaTimeAndECB<E> : ConsumerSystem<EffectConsumerSystemWithDeltaTimeAndECB<E>.ConsumerJob, E>
        where E : struct, IEffectApplierWithDeltaTimeAndECB, IEffect
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public int JobIndex;
            public EntityCommandBuffer.Concurrent ECB;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;
            [ReadOnly] public BufferFromEntity<Child> ChildFromEntity;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {
                    // Apply the effect to the component data.
                    RecursiveChildsApply(effect.Other, effect);
                }
            }

            void RecursiveChildsApply(Entity entity, E effect)
            {
                if (ChildFromEntity.Exists(entity) && effect.ApplyRecursivelyToChildren)
                {
                    var children = ChildFromEntity[entity];
                    for (int i = 0; i < children.Length; i++)
                    {
                        RecursiveChildsApply(children[i].Value, effect);
                    }
                }
                effect.Apply(in DeltaTime, in JobIndex, entity, ref ECB);
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                JobIndex = GetType().GetHashCode(),
                DeltaTime = Time.DeltaTime,
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue,
                ChildFromEntity = GetBufferFromEntity<Child>(true)
            };
        }
    }
}