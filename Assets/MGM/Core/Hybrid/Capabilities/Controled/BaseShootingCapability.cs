using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

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
        public GameObject MuzzleFlashVFX;
        public float Duration;

        [Header("Sound FX")]
        public AudioClip SoundFX;
        [Range(0f, 1f)]
        public float Volume =1f;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Projectile);
            referencedPrefabs.Add(MuzzleFlashVFX);
        }

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            Trigger trigger = new Trigger()
            {
                IsTriggered = false,
                CoolDown = CoolDown,
                TimeSinceLastTrigger = 0
            };


            var vfx = conversionSystem.GetPrimaryEntity(MuzzleFlashVFX);

            CurrentAge currentAge = new CurrentAge()
            {
                Value = 0
            };
            dstManager.AddComponentData(vfx, currentAge);

            MaxAge maxLifeTime = new MaxAge()
            {
                Value = Duration
            };
            dstManager.AddComponentData(vfx, maxLifeTime);

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
