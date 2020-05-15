using System;

namespace Wayn.Mgm.Event.Registry
{
    public interface IRegistry
    {
        /// <summary>
        /// Try to add a new event to the registry.
        /// <para>
        /// If registry already contain an identical event instance, the method : 
        /// <list type="number"> 
        ///     <item><description>returns a reference to the instance stored int the registry.</description>    </item>
        /// </list>
        /// </para>
        /// <para>
        /// If the registry does not contains an identical event instance, the method :
        /// <list type="number"> 
        ///     <item><description>adds the event instance to the registry</description> </item>
        ///     <item><description>calls <see cref="OnNewElementRegistered"/></description> </item>
        ///     <item><description>returns a reference to the stored instance.</description> </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="registryEventInstance">A referenced to the stored intance in the registry.</param>
        /// <returns></returns>
        RegistryEventReference AddEventInstance(IRegistryEvent registryEventInstance);

        /// <summary>
        /// Raise a C# event.
        /// This should be called by <see cref="AddEventInstance(IRegistryEvent)" /> whenever a new event instance is added to the registry.
        /// </summary>
        void OnNewElementRegistered();

        /// <summary>
        /// Add a method to invoke when <see cref="OnNewElementRegistered"/> raise a C# event.
        /// </summary>
        /// <param name="method">The method to register for invokation when an event instance is added to the registry.</param>
        void SubscribeToElementRegisteredEvent(EventHandler method);

        /// <summary>
        /// Remove a method from the list of invoked method when <see cref="OnNewElementRegistered"/> raise a C# event.
        /// </summary>
        /// <param name="method">The method to deregister for invokation when an event instance is added to the registry.</param>
        void UnsubscribeToElementRegisteredEvent(EventHandler method);

        /// <summary>
        /// Extract a copy of all event of a given type referenced by VersionId.
        /// Caching the result is advised.
        /// </summary>
        /// <typeparam name="EVENT_TYPE">The type of event to extract.</typeparam>
        /// <param name="registeredEvents">The NativeHashmap to copy the effects to.</param>
        void GetRegisteredEffects<EVENT_TYPE>(ref Unity.Collections.NativeHashMap<int, EVENT_TYPE> registeredEvents) where EVENT_TYPE : struct, IRegistryEvent;
    }
}

