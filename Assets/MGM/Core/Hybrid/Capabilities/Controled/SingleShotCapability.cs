using System;
using Unity.Entities;
using UnityEngine;

namespace MGM.Core
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

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Setup the minimal component required for a shot mechanic.
            base.SetUpCapabilityParameters(entity, dstManager, conversionSystem);

            // Add a tag component to know that we want to override the base shot system.
            dstManager.AddComponent(entity, typeof(SingleShot));


        }


        private void OnDrawGizmos()
        {
            Gizmos.color= Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 5));
        }
    }

}
