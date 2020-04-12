using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Wayn.Mgm.Events.Registry
{
    public abstract class RegistryEventDispatcher<COMMAND> : JobComponentSystem
        where COMMAND : struct, IEventRegistryCommand
    {
        private List<NativeQueue<COMMAND>> CommandsQueues;
        public NativeMultiHashMap<ulong, COMMAND> CommandsMap;
        private JobHandle JobHandle;
        public JobHandle FinalJobHandle;

        private JobHandle CrossFrameJobHandle;



        public void AddJobHandleForConsumer(JobHandle jh)
        {
            JobHandle = JobHandle.CombineDependencies(JobHandle, jh);
        }

        public void AddConsumerJobHandle(JobHandle jh)
        {
            CrossFrameJobHandle = JobHandle.CombineDependencies(CrossFrameJobHandle, jh);
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
            [ReadOnly]
            public NativeArray<int> TotalCommandCount;

            [WriteOnly]
            public NativeMultiHashMap<ulong, COMMAND> CommandsMap;

            public void Execute()
            {
                CommandsMap.Capacity = TotalCommandCount[0];
            }
        }


        struct CountCommands : IJob
        {
            [ReadOnly]
            public NativeQueue<COMMAND> CommandsQueue;

            public NativeArray<int> TotalCommandCount;

            public void Execute()
            {
                TotalCommandCount[0] += CommandsQueue.Count;
            }
        }

        [BurstCompile]
        struct MapCommands : IJob
        {
         
            public NativeQueue<COMMAND> CommandsQueue;

            [WriteOnly]
            [NativeDisableContainerSafetyRestriction]
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
            

            JobHandle = JobHandle.CombineDependencies(inputDeps, JobHandle, CrossFrameJobHandle);

            if (CommandsMap.IsCreated)
            {
                JobHandle = CommandsMap.Dispose(JobHandle);
            }
            CommandsMap = new NativeMultiHashMap<ulong, COMMAND>(0, Allocator.TempJob);

            // Schedule in sequence the realocation of the necessary memory to handle each commands based on the queues sizes.
            // Not done in parallel as the resize consist of an new allocation and a copy.
            // Doing it in parallel would result in branching allocations.
            NativeArray<int> counter = new NativeArray<int>(1,Allocator.TempJob);
 
            for (int i = 0; i < CommandsQueues.Count; i++)
            {
                JobHandle = new CountCommands()
                {
                    CommandsQueue = CommandsQueues[i],
                    TotalCommandCount = counter
                }.Schedule(JobHandle);
            }

            JobHandle = new AllocateCommandsMap()
            {
                TotalCommandCount = counter,
                CommandsMap = CommandsMap
            }.Schedule(JobHandle);

            counter.Dispose(JobHandle);

            NativeArray<JobHandle> MapperJobHanldes = new NativeArray<JobHandle>(CommandsQueues.Count, Allocator.TempJob);
            var CommandsMapParallelWriter = CommandsMap.AsParallelWriter();
           
            for (int i = 0; i < CommandsQueues.Count; i++)
            {
                MapperJobHanldes[i] = new MapCommands()
                {
                    CommandsMap = CommandsMapParallelWriter,
                    CommandsQueue = CommandsQueues[i]
                }.Schedule(JobHandle);
            }


            FinalJobHandle = JobHandle.CombineDependencies(MapperJobHanldes);


            MapperJobHanldes.Dispose();
            return FinalJobHandle;
        }

    }

}
