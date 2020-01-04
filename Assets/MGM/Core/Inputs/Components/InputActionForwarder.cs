using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputActionForwarder : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] protected GameObject PlayerGameObject;
    protected Entity PlayerEntity;
    protected EntityManager EntityManager;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {   
        PlayerEntity = conversionSystem.GetPrimaryEntity(PlayerGameObject);
        EntityManager = dstManager;
    }

    public abstract void ForwardAction(InputAction.CallbackContext ctx);
}
