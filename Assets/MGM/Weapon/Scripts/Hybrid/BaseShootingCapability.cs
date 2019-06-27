using MGM.Common;
using MGM.Weapon;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace MGM.Core
{
    /// <summary>
    /// Base Shoting capability to inherit from in order to customize the shooting behavior.
    /// </summary>
    public abstract class BaseShootingCapability :   ControledCapability<ShootingInputListener> , IDeclareReferencedPrefabs
    {
        [Header("Base Shot Properties")]
        public GameObject Projectile;
        public float CoolDown;
        public float Speed;
        [Range(-1, 999)]
        public int MagazineCapacity;

        [Header("VFX")]
        public GameObject VFX;
        public float Duration;

        [Header("Sound FX")]
        public AudioClip SoundFX;
        [Range(0f, 1f)]
        public float Volume =1f;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Projectile);
        }

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

       

            Trigger trigger = new Trigger()
            {
                IsTriggered = false,
                CoolDown = CoolDown,
                TimeSinceLastTrigger = 0
            };


            ECS_VFX vfx = new ECS_VFX()
            {
                vfxIndex = VFXRepository.AddVFX(Instantiate(VFX)),
                play = false
            };
            dstManager.AddComponentData(entity, vfx);

            Shot shot = new Shot()
            {
                Projectile = conversionSystem.GetPrimaryEntity(Projectile),
                MuzzleFlashVFX = vfx,
                Speed  = Speed,
                Trigger = trigger
            };
            dstManager.AddComponentData(entity, shot);

            Magazine magazine = new Magazine()
            {
                CurrentCapacity = MagazineCapacity,
                MaxCapacity = MagazineCapacity
            };

            // Add a tag component to know that we want to override the base shot system.
            dstManager.AddComponentData(entity, magazine);

            SoundFXManager.SoundList.Add(SoundFX);

            SoundFX sfx = new SoundFX()
            {
                Index = SoundFXManager.SoundList.IndexOf(SoundFX),
                Volume = Volume
            };
            dstManager.AddComponentData(entity, sfx);


        }
    }

}
