using System;
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

    /// <summary>
    ///  This class handle all lhte complexity of managing the population of the DynamicBuffer with the correct RegistryReference
    /// </summary>
    /// <typeparam name="BUFFER"></typeparam>
    /// <typeparam name="ELEMENT"></typeparam>
    /// <typeparam name="AUTHORING"></typeparam>
    /// <typeparam name="REGISTRY"></typeparam>
    /// 
    public abstract class BaseRegisteryReferenceBufferAuthoring<BUFFER, ELEMENT, AUTHORING, REGISTRY> : BaseRegisteryReferenceBufferAuthoringForCutomEditor, IConvertGameObjectToEntity
        where BUFFER : struct, IEffectReferenceBuffer
        where ELEMENT : IRegistryElement
        where AUTHORING : RegisteryReferenceAuthoring<ELEMENT>
        where REGISTRY : Registry<REGISTRY, ELEMENT>
    {
        [SerializeField]
        public List<AUTHORING> Entries = new List<AUTHORING>();

        protected abstract REGISTRY GetRegisteryInstance();


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Get the effect registry.
            REGISTRY er = GetRegisteryInstance();

            // Add dynamic buffer to store Effect References on the entity
            DynamicBuffer<BUFFER> effectBuffer = dstManager.AddBuffer<BUFFER>(entity);
            // Fore each effect defined in hte inspector
            foreach (AUTHORING effectAuthoring in Entries)
            {
                effectBuffer.Add(new BUFFER() { EffectReference = er.AddEffect(effectAuthoring.Entry) });
            }
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