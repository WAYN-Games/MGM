using Wayn.Mgm.Event;
using Unity.Entities;
using Unity.Jobs;

[UpdateBefore(typeof(EffectConsumerSystemGroup))]
public abstract class EffectJobSystem : JobComponentSystem
{

    protected EffectDisptacherSystem m_EffectBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        m_EffectBufferSystem = World.GetOrCreateSystem<EffectDisptacherSystem>();


    }

    protected void AddJobHandleForConsumer(JobHandle job)
    {
        m_EffectBufferSystem.AddJobHandleFromProducer(job);
    }

}
