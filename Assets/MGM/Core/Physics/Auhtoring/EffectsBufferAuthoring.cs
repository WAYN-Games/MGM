using Wayn.Mgm.Effects;

public abstract class EffectsBufferAuthoring<BUFFER> : BaseRegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectRegistry>
    where BUFFER : struct,IEffectReferenceBuffer
{
    protected override EffectRegistry GetRegisteryInstance()
    {
        return EffectRegistry.Instance;
    }
}
