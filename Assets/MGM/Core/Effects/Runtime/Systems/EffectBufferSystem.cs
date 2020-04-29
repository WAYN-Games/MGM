using Unity.Entities;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public class EffectBufferSystem : RegistryEventDispatcher<EffectCommand>
    {

    }
}
