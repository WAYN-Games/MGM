using Unity.Entities;
using UnityEngine;


namespace MGM
{
    [RequiresEntityConversion]
    public class DieOfOldAge : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float MaxAge = float.MaxValue;
        [SerializeField] private float StartAge = 0;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {


            CurrentAge currentAge = new CurrentAge()
            {
                Value  = StartAge
            };
            dstManager.AddComponentData(entity, currentAge);

            MaxAge maxLifeTime = new MaxAge()
            {
                Value = MaxAge
            };
            dstManager.AddComponentData(entity, maxLifeTime);

        }


    }
}
