using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputActionForwarder<T> : MonoBehaviour, IConvertGameObjectToEntity where T : struct, IComponentData
{
    [SerializeField] protected List<GameObject> PlayerGameObjects = new List<GameObject>();
    protected List<Entity> PlayerEntity = new  List<Entity>();
    protected EntityManager EntityManager;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {   
        foreach(var playerGameObject in PlayerGameObjects)
        {
            PlayerEntity.Add(conversionSystem.GetPrimaryEntity(playerGameObject));
        }
        EntityManager = dstManager;
    }
    
    protected void ForwardAction(T inputData)
    {
        foreach (var playerEntity in PlayerEntity)
        {
            EntityManager.SetComponentData(playerEntity, inputData);
        }
    }

    public abstract void ReadAction(InputAction.CallbackContext ctx);
}
