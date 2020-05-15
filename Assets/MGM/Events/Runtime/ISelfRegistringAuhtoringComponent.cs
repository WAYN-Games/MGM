using Unity.Entities;
namespace Wayn.Mgm.Event.Registry
{
    public interface ISelfRegistringAuhtoringComponent
    {
        void Register(EntityCommandBuffer ecb, Entity entity);
    }
}