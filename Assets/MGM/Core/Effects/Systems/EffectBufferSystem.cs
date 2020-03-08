using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Unity.Entities;
using Unity.Jobs;
using Wayn.Mgm.Effects.Generated;

namespace Wayn.Mgm.Effects
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public class EffectBufferSystem : JobComponentSystem
    {
        public EffectBuffer EffectBuffer;

        /// <summary>
        /// Store the combined JobHandles of the effect initiators.
        /// </summary>
        private JobHandle JobHandle;

        public void AddJobHandleForConsumer(JobHandle jh)
        {
            JobHandle = JobHandle.CombineDependencies(JobHandle, jh);
        }


        internal JobHandle GetJobHandle()
        {
            return JobHandle;
        }

        protected override void OnCreate()
        {
            EffectBuffer = new EffectBuffer(World);

            // TODO proper population of the registery through the editor.
            EffectBuffer.effectRegistry.AddEffectVerion(new Combat.Effects.ChangeHealthEffect() { Amount = -10 });
            EffectBuffer.effectRegistry.AddEffectVerion(new Combat.Effects.DestroyEntityHierarchyEffect());
            EffectBuffer.effectRegistry.AddEffectVerion(new Combat.Effects.DisableEntityHierarchyEffect());
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle = JobHandle.CombineDependencies(inputDeps, JobHandle);
            return JobHandle;
        }
    }
}
