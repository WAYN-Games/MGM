using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

/// <summary>
/// This class hides the complexity of all the types necessary for handeling the population of the buffer.
/// </summary>
/// <typeparam name="BUFFER"></typeparam>
public abstract class RegisteryReferenceBufferAuthoring<BUFFER> : BaseRegisteryReferenceBufferAuthoring<BUFFER, IEffect, EffectAuthoring, EffectRegistry>
        where BUFFER : struct, IEffectReferenceBuffer
    {
        protected override EffectRegistry GetRegisteryInstance()
        {
            return EffectRegistry.Instance;
        } 
    }
namespace Wayn.Mgm.Events.Registry
{

    public interface ISelfRegistringAuhtoringComponent
    {
        void Register(EntityCommandBuffer ecb, Entity entity);
    }

    /// <summary>
    ///  This class handle all lhte complexity of managing the population of the DynamicBuffer with the correct RegistryReference
    /// </summary>
    /// <typeparam name="BUFFER"></typeparam>
    /// <typeparam name="ELEMENT"></typeparam>
    /// <typeparam name="AUTHORING"></typeparam>
    /// <typeparam name="REGISTRY"></typeparam>
    public abstract class BaseRegisteryReferenceBufferAuthoring<BUFFER, ELEMENT, AUTHORING, REGISTRY> : BaseRegisteryReferenceBufferAuthoringForCutomEditor, ISelfRegistringAuhtoringComponent, IConvertGameObjectToEntity
        where BUFFER : struct, IEffectReferenceBuffer
        where ELEMENT : IRegistryElement
        where AUTHORING : RegisteryReferenceAuthoring<ELEMENT>
        where REGISTRY : Registry<REGISTRY, ELEMENT>
    {
        [SerializeField]
        public List<AUTHORING> Entries = new List<AUTHORING>();

        protected abstract REGISTRY GetRegisteryInstance();

        public void Register(EntityCommandBuffer ecb, Entity entity)
        {
            var buffer = ecb.AddBuffer<BUFFER>(entity);
            foreach (var effect in Entries)
            {
                buffer.Add(new BUFFER() { EffectReference = GetRegisteryInstance().AddEffect(effect.Entry) });
            }
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
           
            if(!dstManager.HasComponent<EffectComponentData>(entity)){
                dstManager.AddComponentData(entity, new EffectComponentData());
            }

            EffectComponentData component = dstManager.GetComponentData<EffectComponentData>(entity);
            component.listOfManagedBuffer.Add(this);
            dstManager.SetComponentData(entity,component);
        }
    }
    
    public abstract class BaseRegisteryReferenceBufferAuthoringForCutomEditor : MonoBehaviour
    {

    }


    public abstract class RegisteryReferenceAuthoring<ELEMENT> where ELEMENT : IRegistryElement
    {
        [SerializeReference]
        public ELEMENT Entry;

    }
}