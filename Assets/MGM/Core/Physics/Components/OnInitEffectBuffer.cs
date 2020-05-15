using System;
using Unity.Entities;
using Wayn.Mgm.Event;
using Wayn.Mgm.Event.Registry;

[Serializable]
[InternalBufferCapacity(1)]
public struct OnInitEffectBuffer : IRegistryReferenceBuffer
{
    public RegistryEventReference RegistryEventReference { get; set; }
}
