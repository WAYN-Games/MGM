using System;
using Wayn.Mgm.Event;
using Wayn.Mgm.Event.Registry;
/// <summary>
/// This class hides the complexity of all the types necessary for handeling the population of the buffer.
/// </summary>
/// <typeparam name="BUFFER"></typeparam>
[Serializable]
public abstract class EffectBufferAuthoring<BUFFER> : RegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectComponentDataElement<IEffect, BUFFER>>
        where BUFFER : struct, IRegistryReferenceBuffer
{
}
