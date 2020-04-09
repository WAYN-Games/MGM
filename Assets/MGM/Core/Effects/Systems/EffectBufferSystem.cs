using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Effects
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public class EffectBufferSystem : JobComponentSystem
    {
        private List<NativeQueue<EffectCommand>> EffectQueues;

        public NativeMultiHashMap<ulong, EffectCommand> EffectCommandMap;

        /// <summary>
        /// Store the combined JobHandles of the effect initiators.
        /// </summary>
        private JobHandle JobHandle;

        public JobHandle FinalJobHandle;

        public void AddJobHandleForConsumer(JobHandle jh)
        {
            JobHandle = JobHandle.CombineDependencies(JobHandle, jh);
        }

        public NativeQueue<EffectCommand>.ParallelWriter CreateEffectCommandQueue()
        {
            var EffectQueue = new NativeQueue<EffectCommand>(Allocator.Persistent);
            EffectQueues.Add(EffectQueue);
            return EffectQueue.AsParallelWriter();
        }
        protected override void OnCreate()
        {
            EffectQueues = new List<NativeQueue<EffectCommand>>();
        }

        protected override void OnDestroy()
        {
            foreach (var nativeQueue in EffectQueues)
            {
                nativeQueue.Dispose();
            }
            EffectCommandMap.Dispose();
        }

        struct CountEffectCommand : IJob
        {
            public NativeQueue<EffectCommand> EffectQueue;

            public NativeMultiHashMap<ulong, EffectCommand> Map;

            public void Execute()
            {
                Map.Capacity += EffectQueue.Count;
            }
        }

        [BurstCompile]
        struct MapEffectCommands : IJob
        {
            public NativeQueue<EffectCommand> EffectQueue;
            [WriteOnly]
            public NativeMultiHashMap<ulong, EffectCommand>.ParallelWriter EffectCommandMap;

            public void Execute()
            {

                EffectCommand effectCommand;
                while (EffectQueue.TryDequeue(out effectCommand))
                {
                    EffectCommandMap.Add(effectCommand.EffectReference.TypeId, effectCommand);
                }

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {



            JobHandle = JobHandle.CombineDependencies(inputDeps, JobHandle);

            if (EffectCommandMap.IsCreated)
            {
                EffectCommandMap.Dispose();
            }
            EffectCommandMap = new NativeMultiHashMap<ulong, EffectCommand>(0, Allocator.TempJob);
            NativeArray<JobHandle> ResizeMapJobHanldes = new NativeArray<JobHandle>(EffectQueues.Count, Allocator.Temp);

            for (int i = 0; i < EffectQueues.Count; i++)
            {
                ResizeMapJobHanldes[i] = new CountEffectCommand()
                {
                    EffectQueue = EffectQueues[i],
                    Map = EffectCommandMap
                }.Schedule(JobHandle);
            }

            JobHandle AfterResize = JobHandle.CombineDependencies(ResizeMapJobHanldes);

            ResizeMapJobHanldes.Dispose();

            NativeArray<JobHandle> MapperJobHanldes = new NativeArray<JobHandle>(EffectQueues.Count, Allocator.Temp);

            for (int i = 0; i < EffectQueues.Count; i++)
            {
                MapperJobHanldes[i] = new MapEffectCommands()
                {
                    EffectCommandMap = EffectCommandMap.AsParallelWriter(),
                    EffectQueue = EffectQueues[i]
                }.Schedule(AfterResize);
            }

            FinalJobHandle = JobHandle.CombineDependencies(MapperJobHanldes);


            return FinalJobHandle;
        }

    }
}
