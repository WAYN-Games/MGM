using System;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;
/// <summary>
/// This class hides the complexity of all the types necessary for handeling the population of the buffer.
/// </summary>
/// <typeparam name="BUFFER"></typeparam>
[Serializable]
public abstract class EffectBufferAuthoring<BUFFER> : RegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectRegistry>
        where BUFFER : struct, IRegistryReferenceBuffer
    {
        protected override EffectRegistry GetRegisteryInstance()
        {
            return EffectRegistry.Instance;
        } 
    }
