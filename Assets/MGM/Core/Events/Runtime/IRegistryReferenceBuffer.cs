using Unity.Entities;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{
    public interface IRegistryReferenceBuffer : IBufferElementData
    {
        RegistryReference RegistryEventReference { get; set; }
    }
}