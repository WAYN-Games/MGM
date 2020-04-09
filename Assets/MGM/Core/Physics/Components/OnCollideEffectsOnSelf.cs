using System;
using Unity.Entities;
using Wayn.Mgm.Effects;

[Serializable]
[InternalBufferCapacity(1)]
public struct OnCollideEffectsOnSelfBuffer : IEffectReferenceBuffer
{
    public RegistryReference EffectReference { get; set; }
}

