using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MGM
{
    /// <summary>
    /// Base Shoting capability to inherit from in order to customize the shooting behavior.
    /// </summary>
    public abstract class BaseShootingCapability :   ControledCapability<ShootingInputResponse> , IDeclareReferencedPrefabs
    {
        public GameObject Projectile;
        public float CoolDown;
        /// <summary>
        /// Speed of the projectile.
        /// </summary>
        public float Speed;

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
            Shot shot = new Shot()
            {
                Projectile = conversionSystem.GetPrimaryEntity(Projectile),
                Speed  = Speed,
                Trigger = trigger
            };
            dstManager.AddComponentData(entity, shot);
            

        }
    }

}
