using System;
using Unity.Entities;

namespace MGM
{

    [Serializable]
    [WriteGroup(typeof(Shot))]
    public struct SingleShot : IComponentData
    {
        // No particullar data requiered for the single shot mechanique. Just a component to allow the overriding of the base shot system.
    }


    /// <summary>
    /// Give the capability to shoot a single bullet.
    /// </summary>
    public class SingleShotCapability : BaseShootingCapability
    {
        public int MagazineCapacity;

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Setup the minimal component required for a shot mechanic.
            base.SetUpCapabilityParameters(entity, dstManager, conversionSystem);

            // Add a tag component to know that we want to override the base shot system.
            dstManager.AddComponent(entity, typeof(SingleShot));

            Magazine magazine = new Magazine()
            {
                CurrentCapacity = MagazineCapacity,
                MaxCapacity = MagazineCapacity
            };

            // Add a tag component to know that we want to override the base shot system.
            dstManager.AddComponentData(entity, magazine);
        }
    }

}
