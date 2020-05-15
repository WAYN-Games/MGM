using Unity.Entities;
using Wayn.Mgm.Event.Registry;

namespace Wayn.Mgm.Event
{
    public struct EffectCommand : IEventRegistryCommand
    {
        public Entity Emitter;
        public Entity Target;
        public RegistryEventReference RegistryReference { get ; set; }
    }
}
