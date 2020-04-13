using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerGameObjectReference))]
public abstract class InputActionForwarder<T> : MonoBehaviour, IConvertGameObjectToEntity where T : struct, IComponentData
{
    private List<Entity> PlayerEntity = new  List<Entity>();
    protected EntityManager EntityManager;
    protected Camera PlayerCamera;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        PlayerGameObjectReference PlayerGameObjectReference = GetComponent<PlayerGameObjectReference>();
        PlayerCamera = GetComponent<PlayerInput>().camera;
        
        GameObject PlayerGameObjectRoot = PlayerGameObjectReference.PlayerGameObjectRoot;
        if (PlayerGameObjectRoot == null)
        {
            PlayerGameObjectRoot = gameObject;
        }

        foreach (var playerGameObject in PlayerGameObjectRoot.GetComponentsInChildren(GetAuthoringComponentTypeFromIComponentData(typeof(T))))
        {
            PlayerEntity.Add(conversionSystem.GetPrimaryEntity(playerGameObject));
        }
        EntityManager = dstManager;
    }

    private Type GetAuthoringComponentTypeFromIComponentData(Type IComponentDataType)
    {
        string[] AssemblyQualifiedNameAttributes = IComponentDataType.AssemblyQualifiedName.Split(',');
        string AuthoringComponentAssemblyQualifiedName = $"{IComponentDataType.Name}Authoring";
        for (int i = 1; i < AssemblyQualifiedNameAttributes.Length; i++)
        {
            AuthoringComponentAssemblyQualifiedName = $"{AuthoringComponentAssemblyQualifiedName}, {AssemblyQualifiedNameAttributes[i]}";
        }
        return Type.GetType(AuthoringComponentAssemblyQualifiedName);
    }

    protected void ForwardAction(T inputData)
    {
        foreach (var playerEntity in PlayerEntity)
        {
            if (!EntityManager.Exists(playerEntity)) continue;
            EntityManager.SetComponentData(playerEntity, inputData);
        }
    }

    public abstract void ReadAction(InputAction.CallbackContext ctx);
}
