using Unity.Entities;
using UnityEngine;

namespace MGM
{
    public class AimCapability : ControledCapability<MousePositionResponse>
    {
        
        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Aim());
        }
    }
}
