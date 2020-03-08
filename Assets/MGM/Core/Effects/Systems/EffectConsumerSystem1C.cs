using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Effects
{

    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystem<E, C1> : ConsumerSystem<EffectConsumerSystem<E, C1>.ConsumerJob, E>
        where E : struct, IEffectApplier<C1>, IEffect
        where C1 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {
                    // Apply the effect to the component data.
                    effect.Apply(ref Components);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                Components = GetComponentDataFromEntity<C1>(false),
                ConsumerQueue = ConsumerQueue
            };
        }
    }

    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithECB<E, C1> : ConsumerSystem<EffectConsumerSystemWithECB<E, C1>.ConsumerJob, E>
        where E : struct, IEffectApplierWithECB<C1>, IEffect
        where C1 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public int JobIndex;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public EntityCommandBuffer.Concurrent ECB;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {

                    // Apply the effect to the component data.
                    effect.Apply(in JobIndex, ref ECB, effect.Other, ref Components);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                JobIndex = GetType().GetHashCode(),
                Components = GetComponentDataFromEntity<C1>(false),
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue
            };
        }
    }



    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithDeltaTime<E, C1> : ConsumerSystem<EffectConsumerSystemWithDeltaTime<E, C1>.ConsumerJob, E>
        where E : struct, IEffectApplierWithDeltaTime<C1>, IEffect
        where C1 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public float DeltaTime;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {

                    // Apply the effect to the component data.
                    effect.Apply(in DeltaTime, ref Components);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                DeltaTime = Time.DeltaTime,
                Components = GetComponentDataFromEntity<C1>(false),
                ConsumerQueue = ConsumerQueue
            };
        }
    }


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithDeltaTimeAndECB<E, C1> : ConsumerSystem<EffectConsumerSystemWithDeltaTimeAndECB<E, C1>.ConsumerJob, E>
        where E : struct, IEffectApplierWithDeltaTimeAndECB<C1>, IEffect
        where C1 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public int JobIndex;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public EntityCommandBuffer.Concurrent ECB;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {

                    // Apply the effect to the component data.
                    effect.Apply(in DeltaTime, in JobIndex, ref ECB, effect.Other, ref Components);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                JobIndex = GetType().GetHashCode(),
                DeltaTime = Time.DeltaTime,
                Components = GetComponentDataFromEntity<C1>(false),
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue
            };
        }
    }
}