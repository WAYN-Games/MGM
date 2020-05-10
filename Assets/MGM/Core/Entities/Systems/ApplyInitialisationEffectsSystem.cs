using Unity.Entities;
using Wayn.Mgm.Events;

public class ApplyInitialisationEffectsSystem : SystemBase
{
    private BeginInitializationEntityCommandBufferSystem initialisationSystem;
    private EffectDisptacherSystem effectBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        initialisationSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        effectBufferSystem = World.GetOrCreateSystem<EffectDisptacherSystem>();
    }

    protected override void OnUpdate()
    {
        var PostUpdateCommand = initialisationSystem.CreateCommandBuffer().ToConcurrent();
        var EffectCommandQueue = effectBufferSystem.CreateCommandsQueue();

        Entities.WithName("ApplyInitEffect")
            .WithBurst()
            .ForEach(
               (Entity entity, int entityInQueryIndex, ref DynamicBuffer<OnInitEffectBuffer> initEffectBuffer) =>
               {
                   var enumerator = initEffectBuffer.GetEnumerator();
                   while (enumerator.MoveNext())
                   {
                       var command = enumerator.Current;
                       EffectCommandQueue.Enqueue(new EffectCommand() {
                           Emitter = Entity.Null,
                           Target = entity,
                           RegistryReference = command.RegistryEventReference });
                   }
                   PostUpdateCommand.RemoveComponent<OnInitEffectBuffer>(entityInQueryIndex, entity);
               }
           ).ScheduleParallel();
        
        initialisationSystem.AddJobHandleForProducer(Dependency);
        effectBufferSystem.AddJobHandleFromProducer(Dependency);
    }
}