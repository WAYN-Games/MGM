using Unity.Entities;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    public interface IEffectReferenceBuffer : IBufferElementData
    {
        RegistryReference EffectReference { get; set; }
    }
}