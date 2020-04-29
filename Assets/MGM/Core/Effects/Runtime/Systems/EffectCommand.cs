using Unity.Entities;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    public struct EffectCommand : IEventRegistryCommand
    {
        public Entity Emitter;
        public Entity Target;
        public RegistryReference RegistryReference { get ; set; }
    }

    public interface IEventRegistryCommand
    {
        RegistryReference RegistryReference { get; set; }
    }
}
