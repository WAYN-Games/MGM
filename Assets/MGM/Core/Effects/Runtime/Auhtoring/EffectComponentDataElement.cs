using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;

[Serializable]
public class EffectComponentDataElement<ELEMENT, BUFFER> : RegistryEventComponentDataElement<ELEMENT, BUFFER>
     where ELEMENT : IRegistryElement
     where BUFFER : struct, IRegistryReferenceBuffer
{
    protected override RegistryReference AddEventToRegistry(ELEMENT registeryEvent)
    {
        return EffectRegistry.Instance.AddEffect((IEffect)registeryEvent);
    }
}
