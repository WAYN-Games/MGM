using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Events
{

    public abstract class RegisteredEventConsumer<DISPATCHER,COMMAND, ELEMENT> : SystemBase
        where COMMAND : IEventRegistryCommand
        where ELEMENT : struct, IRegistryElement
        where DISPATCHER : RegistryEventDispatcher<EffectCommand>
    {
        /// <summary>
        /// System that takes care of dispatching the event based on their type.
        /// </summary>
        protected DISPATCHER _dispatcherSystem;
        /// <summary>
        /// The registry containing all the registered event types and instances.
        /// </summary>
        protected IRegistry _registry;
        /// <summary>
        /// A cached map of all the event instanced handled by this consumer.
        /// </summary>
        protected NativeHashMap<int, ELEMENT> _registeredEvents;

        /// <summary>
        /// Indicated whether the _registeredEvents should be refreshed in hte next Onpdate loop.
        /// </summary>
        protected bool _shouldRefreshCache = true;

        protected int _eventTypeId;

        protected override void OnUpdate()
        {
            if (_shouldRefreshCache)
            {
                RefreshRegisteredEffectsCache();
                _shouldRefreshCache = false;
            }

            NativeMultiHashMap<MapKey, EffectCommand>.Enumerator eventCommandEnumerator = _dispatcherSystem.CommandsMap.GetValuesForKey(new MapKey() { Value = _eventTypeId });
            Dependency = JobHandle.CombineDependencies(Dependency, _dispatcherSystem.finalJobHandle);
            Dependency = ScheduleJob(in eventCommandEnumerator, in _registeredEvents);
            _dispatcherSystem.AddConsumerJobHandle(Dependency);
        }

        protected abstract JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator effectCommandEnumerator,
            in NativeHashMap<int, ELEMENT> m_RegisteredEffects);

        /// <summary>
        /// Method to delegate the registery instance retreival to the child class
        /// because the singleton instance cand be retreived from the generic type.
        /// </summary>
        /// <returns>An instance of the registry containing all the registered event types and instances.</returns>
        protected abstract IRegistry GetRegistryInstance();

        /// <summary>
        /// Refesh the cached map of all the event instanced handled by this consumer.
        /// </summary>
        private void RefreshRegisteredEffectsCache()
        {
            _registry = GetRegistryInstance();

            if (_registeredEvents.IsCreated) _registeredEvents.Dispose();

            _registry.GetRegisteredEffects(ref _registeredEvents);
        
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _dispatcherSystem = World.GetOrCreateSystem<DISPATCHER>();
            _eventTypeId = RegistryReference.GetTypeId(typeof(ELEMENT));

            RefreshRegisteredEffectsCache();
            _registry.SubscribeToElementRegisteredEvent(TriggerCacheRefresh);
        }

        private void TriggerCacheRefresh(object sender, EventArgs e)
        {
            _shouldRefreshCache = true;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            _registry.UnsubscribeToElementRegisteredEvent(TriggerCacheRefresh);
            _registeredEvents.Dispose();
        }
    }


    [UpdateInGroup(typeof(EffectConsumerSystemGroup))]
    public abstract class EffectConsumerSystem<ELEMENT> : RegisteredEventConsumer<EffectDisptacherSystem, EffectCommand, ELEMENT>
        where ELEMENT : struct, IEffect
    {
        protected override IRegistry GetRegistryInstance()
        {
            return (IRegistry) EffectRegistry.Instance;
        }
    }
}
