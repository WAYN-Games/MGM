using Unity.Entities;
using UnityEngine;

namespace MGM
{
    public class JumpCapability : ControledCapability<JumpInputResponse>
    {
        [SerializeField] private float Force = 5;
        [SerializeField] private int MaxJumpNumber = 1;



        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var data = new JumpCapabilityParameters
            {
                Force = Force,
                MaxJumpNumber = MaxJumpNumber,
                CurrentJumpCount = 0
            };
            JumpTriger jt = new JumpTriger()
            {

            };

            dstManager.AddComponentData(entity, data);
            dstManager.AddComponentData(entity, jt);
        }
    }
}
