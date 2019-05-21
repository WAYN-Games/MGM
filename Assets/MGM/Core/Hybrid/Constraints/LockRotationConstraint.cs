using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace MGM
{
    [RequiresEntityConversion]
    public class LockRotationConstraint : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private bool3 AxisLocks = new bool3();

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var data = new RotationLock
            {
                AxisLocks = AxisLocks
            };
            dstManager.AddComponentData(entity, data);
        }


    }
}
