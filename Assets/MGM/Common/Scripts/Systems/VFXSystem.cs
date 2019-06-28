
using Unity.Entities;


namespace MGM.Common
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class VFXSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref ECS_VFX vfx)=>
        {
                if (!vfx.play) return;
                VFXRepository.PlayVfxAt(vfx.vfxIndex, vfx.ltw);
                vfx.play = false;
            });


        }
    }
}
