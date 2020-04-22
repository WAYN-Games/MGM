using System;
using Unity.Entities;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

[Serializable]
public abstract class EffectsBufferAuthoring<BUFFER> : BaseRegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectRegistry>
    where BUFFER : struct,IEffectReferenceBuffer
{

    protected override EffectRegistry GetRegisteryInstance()
    {
        return EffectRegistry.Instance;
    }
}
