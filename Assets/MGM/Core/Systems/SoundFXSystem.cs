using MGM;
using Unity.Entities;
using UnityEngine;
namespace MGM.Core
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class SoundFXSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref SoundFX sfx) =>
            {
                if (!sfx.Play) return;

                AudioSource.PlayClipAtPoint(SoundFXManager.SoundList[sfx.Index], sfx.EmmiterPosition, sfx.Volume);

                sfx.Play = false;
            });


        }
    }
}