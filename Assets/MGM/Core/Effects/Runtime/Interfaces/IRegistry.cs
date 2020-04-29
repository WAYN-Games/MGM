namespace Wayn.Mgm.Events.Registry
{
    public interface IRegistry<ELEMENT>
             where ELEMENT : IRegistryElement
    {
        RegistryReference AddEffect(ELEMENT element);
    }
}

