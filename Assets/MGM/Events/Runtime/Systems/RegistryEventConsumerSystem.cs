using System;

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Wayn.Mgm.Event.Registry
{
    /// <summary>
    /// Base class for any registry based event consumer.
    /// </summary>
    /// <typeparam name="DISPATCHER">The type of the dispatcher for the registry based event system..</typeparam>
    /// <typeparam name="COMMAND">The type of the command for the registry based event system.</typeparam>
    /// <typeparam name="ELEMENT">The type of the event consumed by the system.</typeparam>
    public abstract class RegistryEventConsumerSystem<DISPATCHER, COMMAND, ELEMENT> : SystemBase
        where COMMAND : struct, IEventRegistryCommand
        where ELEMENT : struct, IRegistryEvent
        where DISPATCHER : RegistryEventDispatcher<COMMAND>
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

        /// <summary>
        /// Sealed so the derived class can't override the cache refresh and dependency logic.
        /// </summary>
        protected sealed override void OnUpdate()
        {
            // Refresh the cached registered envent when needed.
            if (_shouldRefreshCache)
            {
                RefreshRegisteredEffectsCache();
                _shouldRefreshCache = false;
            }

            // Get all events to process this frame.
            NativeMultiHashMap<MapKey, COMMAND>.Enumerator eventCommandEnumerator = _dispatcherSystem.CommandsMap.GetValuesForKey(new MapKey() { Value = _eventTypeId });

            // Make sure the dispatcher finished is work befor processing the events.
            Dependency = JobHandle.CombineDependencies(Dependency, _dispatcherSystem.finalJobHandle);

            // Ask the derived class to schedule the Event processing.
            Dependency = ScheduleJob(in eventCommandEnumerator, in _registeredEvents);

            // Force the Event processing to finish before the next frame dispatcher starts.
            _dispatcherSystem.AddConsumerJobHandle(Dependency);
        }

        /// <summary>
        /// Method to schedule the event processing job.
        /// </summary>
        /// <param name="effectCommandEnumerator"> An enumerator on all the events to porcess.</param>
        /// <param name="m_RegisteredEffects"> A map of all instance of the processed event type.</param>
        /// <returns></returns>
        protected abstract JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, COMMAND>.Enumerator effectCommandEnumerator,
            in NativeHashMap<int, ELEMENT> m_RegisteredEffects);

        /// <summary>
        /// Method to delegate the registery instance retreival to the child class
        /// because the singleton instance cant be retreived from the generic type.
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

            // Get the dispatcher system for this registry based event system.
            _dispatcherSystem = World.GetOrCreateSystem<DISPATCHER>();

            // Compute the event type id handle by this consumer.
            _eventTypeId = RegistryEventReference.GetTypeId(typeof(ELEMENT));


            RefreshRegisteredEffectsCache();

            // Subscribe to the registry event so that the cache is refreshed whenever a new event instance is added to the registry.
            _registry.SubscribeToElementRegisteredEvent(TriggerCacheRefresh);
        }

        /// <summary>
        /// Set the consumer to refresh the registered event cache on next update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TriggerCacheRefresh(object sender, EventArgs e)
        {
            _shouldRefreshCache = true;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Unsubscribe to the registry event.
            _registry.UnsubscribeToElementRegisteredEvent(TriggerCacheRefresh);

            // Dispose of the cached registered event.
            _registeredEvents.Dispose();
        }
    }
}
