using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;
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
           
            // Fore each effect defined in hte inspector
            List<object> listOfElements = new List<object>();
            foreach (AUTHORING effectAuthoring in Entries)
            {
              //  effectBuffer.Add(new BUFFER() { EffectReference = er.AddEffect(effectAuthoring.Entry) });
                listOfElements.Add(effectAuthoring.Entry);
            }

            EffectComponentData component;
            if (!dstManager.HasComponent<EffectComponentData>(entity))
            {
                dstManager.AddComponentData(entity,new EffectComponentData() { listOfManagedBuffer = new List<ManagedBuffer>()});
            }
            component = dstManager.GetComponentData<EffectComponentData>(entity);

            component.listOfManagedBuffer.Add(new ManagedBuffer() { BufferAssemblyQualifiedName = typeof(BUFFER).AssemblyQualifiedName, Effects = listOfElements });

            dstManager.SetComponentData(entity, component);

        }
    }
    
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(ConvertToEntitySystem))]
    public class RegisterElementsSystem : SystemBase
    {
        private EntityQuery m_PrefabsQuery;
        private EntityQuery m_EntitiesQuery;
        private EndInitializationEntityCommandBufferSystem m_EndInitializationEntityCommandBufferSystem;
        private ConcurrentDictionary<string, MethodInfo> MethodCache;

        protected override void OnCreate()
        {
            base.OnCreate();
            MethodCache = new ConcurrentDictionary<string, MethodInfo>();

            m_PrefabsQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly< EffectComponentData>(),
                ComponentType.ReadOnly< Prefab>()
                }
            });

            m_EntitiesQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly< EffectComponentData>()
                }
            });
           
            m_EndInitializationEntityCommandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            Profiler.BeginSample("RegisterElementsSystem.OnUpdate()");
            List<Task> Tasks = new List<Task>();
            
            NativeArray<Entity> prefabs = m_PrefabsQuery.ToEntityArray(Allocator.TempJob);
            RemapBuffers(prefabs.GetEnumerator(), Tasks);
            prefabs.Dispose();

            
           
            NativeArray<Entity> entities = m_EntitiesQuery.ToEntityArray(Allocator.TempJob);
            RemapBuffers(entities.GetEnumerator(), Tasks);
            entities.Dispose();

            Task.WaitAll(Tasks.ToArray());
            Profiler.EndSample();

        }



        private void RemapBuffers(NativeArray<Entity>.Enumerator enumerator, List<Task> Tasks)
        {
       
            while (enumerator.MoveNext())
            {
                Entity entity = enumerator.Current;
                Tasks.Add(Task.Run(() => ProcessEntity(entity, m_EndInitializationEntityCommandBufferSystem.CreateCommandBuffer())));
            }
      
        }
        private void ProcessEntity(Entity entity, EntityCommandBuffer ecb)
        {
            EffectComponentData EffectComponentData = EntityManager.GetComponentData<EffectComponentData>(entity);
            foreach (ManagedBuffer managedBuffer in EffectComponentData.listOfManagedBuffer)
            {
                Type bufferType = Type.GetType(managedBuffer.BufferAssemblyQualifiedName);
                object buffer = MethodCache.GetOrAdd(managedBuffer.BufferAssemblyQualifiedName + "AddBuffer",
                        typeof(EntityCommandBuffer).GetMethod("AddBuffer").MakeGenericMethod(new Type[] { bufferType })).Invoke(ecb, new object[] { entity });
                
                MethodInfo AddBufferElementMethod = MethodCache.GetOrAdd(buffer.GetType().AssemblyQualifiedName + "Add", buffer.GetType().GetMethod("Add"));

                List<object> elements = managedBuffer.Effects;
                foreach (IEffect effect in elements)
                {
                    IEffectReferenceBuffer b = Activator.CreateInstance(bufferType) as IEffectReferenceBuffer;
                    b.EffectReference = EffectRegistry.Instance.AddEffect(effect);
                    AddBufferElementMethod.Invoke(buffer, new object[] { b });
                }
            }
            ecb.RemoveComponent(entity, typeof(EffectComponentData));
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