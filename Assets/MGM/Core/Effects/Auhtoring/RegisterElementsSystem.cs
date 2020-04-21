using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;
namespace Wayn.Mgm.Events.Registry
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(ConvertToEntitySystem))]
    public class RegisterElementsSystem : SystemBase
    {
        private EntityQuery m_PrefabsQuery;
        private EntityQuery m_EntitiesQuery;
        private EndInitializationEntityCommandBufferSystem m_EndInitializationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabsQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly<EffectComponentData>(),
                ComponentType.ReadOnly<Prefab>()
                }
            });

            m_EntitiesQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                ComponentType.ReadOnly<EffectComponentData>()
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
            foreach(var e in EffectComponentData.listOfManagedBuffer)
            {
                e.Register(ecb, entity);
            }
           
            ecb.RemoveComponent(entity, typeof(EffectComponentData));
        }
    }
}