using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    public interface IEventRegistryCommand
    {
        RegistryReference RegistryReference { get; set; }
    }
}
