using System;
using Unity.Entities;
using Wayn.Mgm.Effects;

[Serializable]
[InternalBufferCapacity(1)]
public struct OnCollideEffectsOnOtherBuffer : IEffectReferenceBuffer
{
    public EffectReference EffectReference { get; set; }
}
