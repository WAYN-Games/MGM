using MGM.Weapon;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;


namespace MGM.Core
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
