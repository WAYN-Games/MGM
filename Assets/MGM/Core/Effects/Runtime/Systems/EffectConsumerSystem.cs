using Unity.Entities;
using Wayn.Mgm.Event.Registry;

namespace Wayn.Mgm.Event
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public abstract class EffectConsumerSystem<ELEMENT> : RegistryEventConsumerSystem<EffectDisptacherSystem, EffectCommand, ELEMENT>
        where ELEMENT : struct, IEffect
    {
        protected override IRegistry GetRegistryInstance()
        {
            return EffectRegistry.Instance;
        }
    }
}
