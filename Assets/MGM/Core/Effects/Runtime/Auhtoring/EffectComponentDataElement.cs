using Wayn.Mgm.Event;
using System;
using Wayn.Mgm.Event.Registry;

[Serializable]
public class EffectComponentDataElement<ELEMENT, BUFFER> : RegistryEventComponentDataElement<ELEMENT, BUFFER>
     where ELEMENT : IRegistryEvent
     where BUFFER : struct, IRegistryReferenceBuffer
{

    protected override RegistryEventReference AddEventToRegistry(ELEMENT registeryEvent)
    {
        return EffectRegistry.Instance.AddEventInstance((IEffect)registeryEvent);
    }
}
