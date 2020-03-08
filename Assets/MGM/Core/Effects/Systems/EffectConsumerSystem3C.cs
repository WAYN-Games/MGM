using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
namespace Wayn.Mgm.Effects
{

    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystem<E, C1, C2, C3> : ConsumerSystem<EffectConsumerSystem<E, C1, C2, C3>.ConsumerJob, E>
        where E : struct, IEffectApplier<C1, C2, C3>, IEffect
        where C1 : struct, IComponentData
        where C2 : struct, IComponentData
        where C3 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public ComponentDataFromEntity<C2> Components2;
            public ComponentDataFromEntity<C3> Components3;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {
                    // Apply the effect to the component data.
                    effect.Apply(ref Components, ref Components2, ref Components3);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                Components = GetComponentDataFromEntity<C1>(false),
                Components2 = GetComponentDataFromEntity<C2>(false),
                Components3 = GetComponentDataFromEntity<C3>(false),
                ConsumerQueue = ConsumerQueue
            };
        }
    }

    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithECB<E, C1, C2, C3> : ConsumerSystem<EffectConsumerSystemWithECB<E, C1, C2, C3>.ConsumerJob, E>
        where E : struct, IEffectApplierWithECB<C1, C2, C3>, IEffect
        where C1 : struct, IComponentData
        where C2 : struct, IComponentData
        where C3 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public int JobIndex;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public ComponentDataFromEntity<C2> Components2;
            public ComponentDataFromEntity<C3> Components3;
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
                    effect.Apply(in JobIndex, ref ECB, effect.Other, ref Components, ref Components2, ref Components3);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                JobIndex = GetType().GetHashCode(),
                Components = GetComponentDataFromEntity<C1>(false),
                Components2 = GetComponentDataFromEntity<C2>(false),
                Components3 = GetComponentDataFromEntity<C3>(false),
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue
            };
        }
    }



    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithDeltaTime<E, C1, C2, C3> : ConsumerSystem<EffectConsumerSystemWithDeltaTime<E, C1, C2, C3>.ConsumerJob, E>
        where E : struct, IEffectApplierWithDeltaTime<C1, C2, C3>, IEffect
        where C1 : struct, IComponentData
        where C2 : struct, IComponentData
        where C3 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public float DeltaTime;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public ComponentDataFromEntity<C2> Components2;
            public ComponentDataFromEntity<C3> Components3;

            // Queue of all effect to apply.
            public NativeQueue<E> ConsumerQueue;

            public void Execute()
            {
                // Loop through all queued effects.
                E effect;

                while (ConsumerQueue.TryDequeue(out effect))
                {

                    // Apply the effect to the component data.
                    effect.Apply(in DeltaTime, ref Components, ref Components2, ref Components3);
                }
            }
        }
        protected override ConsumerJob CreateJob()
        {
            return new ConsumerJob()
            {
                DeltaTime = Time.DeltaTime,
                Components = GetComponentDataFromEntity<C1>(false),
                Components2 = GetComponentDataFromEntity<C2>(false),
                Components3 = GetComponentDataFromEntity<C3>(false),
                ConsumerQueue = ConsumerQueue
            };
        }
    }


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    [UpdateAfter(typeof(EffectBufferSystem))]
    public abstract class EffectConsumerSystemWithDeltaTimeAndECB<E, C1, C2, C3> : ConsumerSystem<EffectConsumerSystemWithDeltaTimeAndECB<E, C1, C2, C3>.ConsumerJob, E>
        where E : struct, IEffectApplierWithDeltaTimeAndECB<C1, C2, C3>, IEffect
        where C1 : struct, IComponentData
        where C2 : struct, IComponentData
        where C3 : struct, IComponentData
    {
        [BurstCompile]
        public struct ConsumerJob : IJob
        {
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public int JobIndex;
            // The array of potentially affected components.
            public ComponentDataFromEntity<C1> Components;
            public ComponentDataFromEntity<C2> Components2;
            public ComponentDataFromEntity<C3> Components3;
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
                    effect.Apply(in DeltaTime, in JobIndex, ref ECB, effect.Other, ref Components, ref Components2, ref Components3);
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
                Components2 = GetComponentDataFromEntity<C2>(false),
                Components3 = GetComponentDataFromEntity<C3>(false),
                ECB = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                ConsumerQueue = ConsumerQueue
            };
        }
    }
}