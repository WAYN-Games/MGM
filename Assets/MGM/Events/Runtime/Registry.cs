using System;
using System.Collections.Concurrent;
using Unity.Collections;

namespace Wayn.Mgm.Event.Registry
{
    public abstract class Registry<T> : IRegistry
        where T : Registry<T>
    {
        private static readonly Lazy<T> Lazy =
            new Lazy<T>(Init 
        );
          
        private static T Init()
        {
            T i = Activator.CreateInstance(typeof(T), true) as T;
            i.registar = new ConcurrentDictionary<int, ConcurrentDictionary<int, IRegistryEvent>>();
            return i;
        }

        private event EventHandler NewElementRegistered;

        /// <inheritdoc/>
        public void OnNewElementRegistered()
        {
            EventArgs e = null;
            NewElementRegistered(this,e);
        }


        public static T Instance => Lazy.Value;

        /// <summary>
        /// Tree to store all effect by Type and Version.
        /// </summary>
        protected ConcurrentDictionary<int, ConcurrentDictionary<int, IRegistryEvent>> registar;

        /// <inheritdoc/>
        public RegistryEventReference AddEventInstance(IRegistryEvent effect)
        {
            int effectTypeId = RegistryEventReference.GetTypeId(effect.GetType());
            int effectVersionId = RegistryEventReference.GetEffectInstanceId(effect);

            

            // Lazy initialzation
            if (!registar.ContainsKey(effectTypeId))
            {
                // Using Concurrent version of the dictionary to ovoid problem of reading while writing event though it should not happen.
                registar[effectTypeId] = new ConcurrentDictionary<int, IRegistryEvent>();
            }

            // Avoid duplicates
            if (!registar[effectTypeId].ContainsKey(effectVersionId))
            {
                registar[effectTypeId].TryAdd(effectVersionId, effect);
                OnNewElementRegistered();
            }
            
            return new RegistryEventReference() { TypeId = effectTypeId, VersionId = effectVersionId };
        }



        /// <inheritdoc/>
        public void SubscribeToElementRegisteredEvent(EventHandler method)
        {
            NewElementRegistered += method;
        }

        /// <inheritdoc/>
        public void UnsubscribeToElementRegisteredEvent(EventHandler method)
        {
            NewElementRegistered -= method;
        }

        /// <summary>
        /// Extract a copy of all event of a given type referenced by VersionId.
        /// Caching the result is advised.
        /// </summary>
        /// <typeparam name="EVENT_TYPE">The type of event to extract.</typeparam>
        /// <param name="registeredEvents">The NativeHashmap to copy the effects to.</param>
        public void GetRegisteredEffects<EVENT_TYPE>(ref NativeHashMap<int, EVENT_TYPE> registeredEvents) where EVENT_TYPE : struct, IRegistryEvent
        {
            int effectTypeId = RegistryEventReference.GetTypeId(typeof(EVENT_TYPE));
            // If no effect are registered for that type return.
            if (!registar.ContainsKey(effectTypeId))
            {
                registeredEvents = new NativeHashMap<int, EVENT_TYPE>(0, Allocator.Persistent);
                return;
            }


            ConcurrentDictionary<int, IRegistryEvent> effects;
            registar.TryGetValue(effectTypeId, out effects);

            registeredEvents = new NativeHashMap<int, EVENT_TYPE>(effects.Count, Allocator.Persistent);

            var effectEnumerator = effects.GetEnumerator();

            // Copy all effect to the result NativeHashMap.
            while (effectEnumerator.MoveNext())
            {
                var current = effectEnumerator.Current;

                registeredEvents.TryAdd(current.Key, (EVENT_TYPE)current.Value);
            }
        }
    }
}

