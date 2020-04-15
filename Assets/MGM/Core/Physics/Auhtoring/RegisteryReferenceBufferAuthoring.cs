using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unity.Collections;
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
           // DynamicBuffer<BUFFER> effectBuffer = dstManager.AddBuffer<BUFFER>(entity);
            // Fore each effect defined in hte inspector
            List<ELEMENT> listOfElements = new List<ELEMENT>();
            foreach (AUTHORING effectAuthoring in Entries)
            {
              //  effectBuffer.Add(new BUFFER() { EffectReference = er.AddEffect(effectAuthoring.Entry) });
                listOfElements.Add(effectAuthoring.Entry);
            }

            EffectComponentData<ELEMENT> component;
            if (!dstManager.HasComponent<EffectComponentData<ELEMENT>>(entity))
            {
                dstManager.AddComponentData(entity,new EffectComponentData<ELEMENT>() { listOfManagedBuffer = new List<ManagedBuffer<ELEMENT>>()});
            }
            component = dstManager.GetComponentData<EffectComponentData<ELEMENT>>(entity);

            component.listOfManagedBuffer.Add(new ManagedBuffer<ELEMENT>() { BufferAssemblyQualifiedName = typeof(BUFFER).AssemblyQualifiedName, Effects = listOfElements });

            dstManager.SetComponentData(entity, component);

        }
    }
    
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(ConvertToEntitySystem))]
    public class RegisterElementsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            NativeArray<Entity> prefabs = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly< EffectComponentData<IEffect>>(),
                ComponentType.ReadOnly< Prefab>()
                }
            }).ToEntityArray(Allocator.TempJob);

            var prefabEnumerator = prefabs.GetEnumerator();



            RemapBuffers(prefabEnumerator);

            prefabs.Dispose();

            var t1 = sw.ElapsedTicks;
            sw.Reset();

            UnityEngine.Debug.Log($"Prefabs {t1 / 10000} ms");

            NativeArray<Entity> entities = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly< EffectComponentData<IEffect>>(),
                }
            }).ToEntityArray(Allocator.TempJob);

            var entitiesEnumerator = entities.GetEnumerator();



            RemapBuffers(entitiesEnumerator);

            entities.Dispose();

            sw.Stop();

            var t2 = sw.ElapsedTicks;
            sw.Stop();

            UnityEngine.Debug.Log($"Entities {t2 / 10000} ms");
            UnityEngine.Debug.Log($"Total {(t1+t2) / 10000} ms");
        }

        private void RemapBuffers(NativeArray<Entity>.Enumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                Entity entity = enumerator.Current;
                EffectComponentData<IEffect> EffectComponentData = EntityManager.GetComponentData<EffectComponentData<IEffect>>(entity);
                foreach (ManagedBuffer<IEffect> managedBuffer in EffectComponentData.listOfManagedBuffer)
                {
                    Type bufferType = Type.GetType(managedBuffer.BufferAssemblyQualifiedName);
    
                    object buffer;
                    if (!EntityManager.HasComponent(entity, bufferType))
                    {
                        buffer = typeof(EntityManager).GetMethod("AddBuffer").MakeGenericMethod(new Type[] { bufferType }).Invoke(EntityManager, new object[] { entity });
                        UnityEngine.Debug.Log(buffer == null);
                        UnityEngine.Debug.Log(buffer == default);
                    }
                    else
                    {
                        buffer = typeof(EntityManager).GetMethod("GetBuffer").MakeGenericMethod(new Type[] { bufferType }).Invoke(EntityManager, new object[] { entity });
                    }

                    MethodInfo m = buffer.GetType().GetMethod("Add");
     
                    List<IEffect> elements = managedBuffer.Effects;
                    foreach (IEffect effect in elements)
                    {
                        IEffectReferenceBuffer b = Activator.CreateInstance(bufferType) as IEffectReferenceBuffer;
                        b.EffectReference = EffectRegistry.Instance.AddEffect(effect);
                        m.Invoke(buffer, new object[] { b });
                    }
                }
                EntityManager.RemoveComponent<EffectComponentData<IEffect>>(entity);
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