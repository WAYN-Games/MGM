using System;
using Unity.Entities;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

[Serializable]
[InternalBufferCapacity(1)]
public struct OnInitEffectBuffer : IRegistryReferenceBuffer
{
    public RegistryReference RegistryEventReference { get; set; }
}
