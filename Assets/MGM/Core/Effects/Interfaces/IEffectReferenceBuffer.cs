using Unity.Entities;
namespace Wayn.Mgm.Effects
{
    public interface IEffectReferenceBuffer : IBufferElementData
    {
        RegistryReference EffectReference { get; set; }
    }
}