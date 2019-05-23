using System;
using Unity.Entities;

namespace MGM
{
    [Serializable]
    [WriteGroup(typeof(Shot))]
    public struct SingleShot : IComponentData
    {

    }


    /// <summary>
    /// Give the capability to shoot a single bullet.
    /// </summary>
    public class SingleShotCapability : BaseShootingCapability
    {


        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            base.SetUpCapabilityParameters(entity, dstManager, conversionSystem);

            SingleShot ss = new SingleShot()
            {

            };

            dstManager.AddComponentData(entity, ss);
        }
    }

}
