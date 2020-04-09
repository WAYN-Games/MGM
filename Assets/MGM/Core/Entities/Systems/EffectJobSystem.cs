using Wayn.Mgm.Effects;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[UpdateBefore(typeof(EffectConsumerSystemGroup))]
public abstract class EffectJobSystem : JobComponentSystem
{

    protected NativeQueue<EffectCommand>.ParallelWriter m_EffectCommandQueue;
    private EffectBufferSystem m_EffectBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        if (m_EffectBufferSystem == null)
        {
            m_EffectBufferSystem = World.GetOrCreateSystem<EffectBufferSystem>();
            m_EffectCommandQueue = m_EffectBufferSystem.CreateEffectCommandQueue();
        }
    }

    protected void AddJobHandleForConsumer(JobHandle job)
    {
        m_EffectBufferSystem.AddJobHandleForConsumer(job);
    }

}
