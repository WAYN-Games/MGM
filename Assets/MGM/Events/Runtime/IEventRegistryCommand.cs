namespace Wayn.Mgm.Event.Registry
{
    public interface IEventRegistryCommand
    {
        RegistryEventReference RegistryReference { get; set; }
    }
}
