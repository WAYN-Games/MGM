using Unity.Entities;
using Wayn.Mgm.Event.Registry;

namespace Wayn.Mgm.Event
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public class EffectDisptacherSystem : RegistryEventDispatcher<EffectCommand>
    {

    }
}
