using System;
using Wayn.Mgm.Event;
using Wayn.Mgm.Event.Registry;

[Serializable]
public abstract class EffectsBufferAuthoring<BUFFER> : RegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectComponentDataElement<IEffect,BUFFER>>
    where BUFFER : struct,IRegistryReferenceBuffer
{

}
