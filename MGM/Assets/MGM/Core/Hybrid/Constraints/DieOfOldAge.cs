using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    [RequiresEntityConversion]
    [RequireComponent(typeof(Age))]
    public class DieOfOldAge : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float MaxAge = float.MaxValue;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            MaxAge maxLifeTime = new MaxAge()
            {
                Value = MaxAge
            };
            dstManager.AddComponentData(entity, maxLifeTime);
        }


    }
}
