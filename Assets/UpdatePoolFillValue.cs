using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePoolFillValue : MonoBehaviour,IConvertGameObjectToEntity
{
    public Slider Fill;

    public GameObject Pool;

    private Entity Entity;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Entity = conversionSystem.GetPrimaryEntity(Pool);
    }

    void Start()
    {
        if(Fill == null)
        {
            Fill = GetComponent<Slider>();
        }
    }

    void FixedUpdate()
    {
        if (Entity.Null.Equals(Entity)) return;

        Health health = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Health>(Entity);
        Fill.value = health.Value / health.MaxValue;

        if(Fill.value <= 0)
        {
            Fill.gameObject.SetActive(false);
        }
    }
}

