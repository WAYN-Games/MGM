using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpdatePoolFillValue<T> : MonoBehaviour,IConvertGameObjectToEntity where T : struct,IPool
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

        T pool = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(Entity); 
        Fill.value = pool.Value / pool.MaxValue;

        if(Fill.value <= 0)
        {
            Fill.gameObject.SetActive(false);
        }
        else
        {
            Fill.gameObject.SetActive(true);
        }
    }
}

