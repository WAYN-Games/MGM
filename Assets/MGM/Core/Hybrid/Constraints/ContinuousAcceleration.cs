using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    [RequiresEntityConversion]
    public class ContinuousAcceleration : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float LinearAccelerationFactor = 0f;
        [SerializeField] private float AngularAccelerationFactor = 0f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var data = new Acceleration
            {
                Linear = LinearAccelerationFactor,
                Angular = AngularAccelerationFactor
            };
            dstManager.AddComponentData(entity, data);
        }


    }
}
