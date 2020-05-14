using Unity.Entities;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public abstract class EffectConsumerSystem<ELEMENT> : RegisteredEventConsumerSystem<EffectDisptacherSystem, EffectCommand, ELEMENT>
        where ELEMENT : struct, IEffect
    {
        protected override IRegistry GetRegistryInstance()
        {
            return EffectRegistry.Instance;
        }
    }
}
