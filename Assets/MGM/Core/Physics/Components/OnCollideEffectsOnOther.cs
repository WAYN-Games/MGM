using System;
using Unity.Entities;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

[Serializable]
[InternalBufferCapacity(1)]
public struct OnCollideEffectsOnOtherBuffer : IEffectReferenceBuffer
{
    public RegistryReference EffectReference { get; set; }
}
