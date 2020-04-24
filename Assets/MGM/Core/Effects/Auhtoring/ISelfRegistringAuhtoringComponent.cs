using Unity.Entities;
namespace Wayn.Mgm.Events.Registry
{
    public interface ISelfRegistringAuhtoringComponent
    {
        void Register(EntityCommandBuffer ecb, Entity entity);
    }
}