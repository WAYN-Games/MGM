using Unity.Entities;
using UnityEngine;


namespace MGM
{
    [RequiresEntityConversion]
    public class DealDamageOnColision : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private int DamageDelt = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Damage damage = new Damage()
            {
                Value = DamageDelt
            };

            dstManager.AddComponentData(entity, damage);
            dstManager.AddComponent(entity,typeof(DestroyOnColision));
        }


    }
}
