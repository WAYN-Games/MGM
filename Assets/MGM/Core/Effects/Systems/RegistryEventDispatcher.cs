using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Events.Registry
{
    public abstract class RegistryEventDispatcher<COMMAND> : JobComponentSystem
        where COMMAND : struct, IEventRegistryCommand
    {
        private List<NativeQueue<COMMAND>> CommandsQueues;
        public NativeMultiHashMap<ulong, COMMAND> CommandsMap;
        private JobHandle JobHandle;
        public JobHandle FinalJobHandle;

        public void AddJobHandleForConsumer(JobHandle jh)
        {
            JobHandle = JobHandle.CombineDependencies(JobHandle, jh);
        }
        public NativeQueue<COMMAND>.ParallelWriter CreateCommandsQueue()
        {
            NativeQueue<COMMAND> NewCommandsQueue = new NativeQueue<COMMAND>(Allocator.Persistent);
            CommandsQueues.Add(NewCommandsQueue);
            return NewCommandsQueue.AsParallelWriter();
        }
        protected override void OnCreate()
        {
            CommandsQueues = new List<NativeQueue<COMMAND>>();
        }

        protected override void OnDestroy()
        {
            foreach (NativeQueue<COMMAND> CommandsQueue in CommandsQueues)
            {
                CommandsQueue.Dispose();
            }
            CommandsMap.Dispose();
        }

        struct AllocateCommandsMap : IJob
        {
            public NativeQueue<COMMAND> CommandsQueue;

            public NativeMultiHashMap<ulong, COMMAND> CommandsMap;

            public void Execute()
            {
                CommandsMap.Capacity += CommandsQueue.Count;
            }
        }

        [BurstCompile]
        struct MapCommands : IJob
        {
            public NativeQueue<COMMAND> CommandsQueue;

            [WriteOnly]
            public NativeMultiHashMap<ulong, COMMAND>.ParallelWriter CommandsMap;

            public void Execute()
            {

                COMMAND command;
                while (CommandsQueue.TryDequeue(out command))
                {
                    CommandsMap.Add(command.RegistryReference.TypeId, command);
                }

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            JobHandle = JobHandle.CombineDependencies(inputDeps, JobHandle);

            if (CommandsMap.IsCreated)
            {
                CommandsMap.Dispose();
            }
            CommandsMap = new NativeMultiHashMap<ulong, COMMAND>(0, Allocator.TempJob);
            NativeArray<JobHandle> ResizeMapJobHanldes = new NativeArray<JobHandle>(CommandsQueues.Count, Allocator.Temp);

            for (int i = 0; i < CommandsQueues.Count; i++)
            {
                ResizeMapJobHanldes[i] = new AllocateCommandsMap()
                {
                    CommandsQueue = CommandsQueues[i],
                    CommandsMap = CommandsMap
                }.Schedule(JobHandle);
            }

            JobHandle AfterResize = JobHandle.CombineDependencies(ResizeMapJobHanldes);

            ResizeMapJobHanldes.Dispose();

            NativeArray<JobHandle> MapperJobHanldes = new NativeArray<JobHandle>(CommandsQueues.Count, Allocator.Temp);

            for (int i = 0; i < CommandsQueues.Count; i++)
            {
                MapperJobHanldes[i] = new MapCommands()
                {
                    CommandsMap = CommandsMap.AsParallelWriter(),
                    CommandsQueue = CommandsQueues[i]
                }.Schedule(AfterResize);
            }

            FinalJobHandle = JobHandle.CombineDependencies(MapperJobHanldes);


            return FinalJobHandle;
        }

    }

}
