using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    [RequiresEntityConversion]
    public class Age : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float StartAge = 0;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            CurrentAge currentAge = new CurrentAge()
            {
                Value = StartAge
            };
            dstManager.AddComponentData(entity, currentAge);
           
        }
    }
}
