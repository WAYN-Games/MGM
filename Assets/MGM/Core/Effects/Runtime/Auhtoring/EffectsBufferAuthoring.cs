using System;
using Unity.Entities;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

[Serializable]
public abstract class EffectsBufferAuthoring<BUFFER> : RegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectRegistry,EffectComponentDataElement<IEffect,BUFFER>>
    where BUFFER : struct,IRegistryReferenceBuffer
{

    protected override EffectRegistry GetRegisteryInstance()
    {
        return EffectRegistry.Instance;
    }
}
