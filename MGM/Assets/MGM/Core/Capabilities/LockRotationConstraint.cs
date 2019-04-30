using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    [RequiresEntityConversion]
    public class LockRotationConstraint : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var data = new LockRotationTag
            {
            };

            dstManager.AddComponentData(entity, data);
        }


    }
}
