using MGM.Common;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace MGM.Core
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(VFXSystem))]
    public class CleanUpVFXSystem : JobComponentSystem
    {

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            List<VFXRepository.SpawnedVFX> VFXPlaying = VFXRepository.VFXPlaying;

            for (int i = VFXPlaying.Count-1; i >= 0; i--)
            {

                VisualEffect ve = VFXPlaying[i].VFX.GetComponent<VisualEffect>();

                if (ve.aliveParticleCount != 0)
                {
                    VFXPlaying[i].FirstParticuleSpawned = true;
                }

                if (VFXPlaying[i].FirstParticuleSpawned && ve.aliveParticleCount == 0)
                {
                    GameObject.Destroy(VFXPlaying[i].VFX);
                    VFXPlaying.RemoveAt(i);
                }
            }

            return inputDeps;
        }
    }
}
