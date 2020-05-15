using Unity.Entities;

namespace Wayn.Mgm.Event.Registry
{
    public interface IRegistryReferenceBuffer : IBufferElementData
    {
        RegistryEventReference RegistryEventReference { get; set; }
    }
}