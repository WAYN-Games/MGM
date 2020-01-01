using Unity.Entities;
using UnityEngine;


namespace MGM
{
    [RequiresEntityConversion]
    public class HealthStat : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private int MaxHealth = 1;
        [SerializeField] private int Health = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            MaxHealth maxHealth = new MaxHealth()
            {
                Value = MaxHealth
            };

            dstManager.AddComponentData(entity, maxHealth);

            Health health = new Health()
            {
                Value = Health
            };


            dstManager.AddComponentData(entity, health);

            conversionSystem.AddLinkedEntityGroup(gameObject);



        }


    }

}
