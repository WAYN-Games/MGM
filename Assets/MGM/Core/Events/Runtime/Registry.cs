using System;
using System.Collections.Concurrent;
using Unity.Collections;
using UnityEngine;

namespace Wayn.Mgm.Events.Registry
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
            i.registar = new ConcurrentDictionary<int, ConcurrentDictionary<int, IRegistryElement>>();
            return i;
        }

        public event EventHandler NewElementRegistered;

        protected void OnNewElementRegistered()
        {
            EventArgs e = null;
            NewElementRegistered(this,e);
        }


        public static T Instance => Lazy.Value;
        /// <summary>
        /// Tree to store all effect by Type and Version.
        /// </summary>
        protected ConcurrentDictionary<int, ConcurrentDictionary<int, IRegistryElement>> registar;



    /// <summary>
    /// Add an effect to the registery and return its EffectReference.
    /// Effect taht have the same type and values are registered only once under the same EffectReference.
    /// </summary>
    /// <param name="effect">The effect to register.</param>
    /// <returns><see cref="RegistryReference"/> A unique reference to that version of the effect.</returns>
    public RegistryReference AddEffect(IRegistryElement effect)
        {
            int effectTypeId = RegistryReference.GetTypeId(effect.GetType());
            int effectVersionId = RegistryReference.GetEffectInstanceId(effect);

            

            // Lazy initialzation
            if (!registar.ContainsKey(effectTypeId))
            {
                // Using Concurrent version of the dictionary to ovoid problem of reading while writing event though it should not happen.
                registar[effectTypeId] = new ConcurrentDictionary<int, IRegistryElement>();
            }

            // Avoid duplicates
            if (!registar[effectTypeId].ContainsKey(effectVersionId))
            {
                registar[effectTypeId].TryAdd(effectVersionId, effect);
            }
            OnNewElementRegistered();
            return new RegistryReference() { TypeId = effectTypeId, VersionId = effectVersionId };
        }

      
        /// <summary>
        /// Extract a copy of all effect of a given type referenced by VersionId.
        /// Caching the result is advised.
        /// </summary>
        /// <typeparam name="T">The type of effect to extract.</typeparam>
        /// <param name="type">The type of effect to extract.</param>
        /// <param name="result">The NativeHashmap to copy the effects to.</param>
        #pragma warning disable CS0693

        public void GetRegisteredEffects<ELEMENT>(ref NativeHashMap<int, ELEMENT> result) where ELEMENT : struct, IRegistryElement
        #pragma warning restore CS0693
        {
            int effectTypeId = RegistryReference.GetTypeId(typeof(ELEMENT));
            // If no effect are registered for that type return.
            if (!registar.ContainsKey(effectTypeId))
            {
                result = new NativeHashMap<int, ELEMENT>(0, Allocator.Persistent);
                return;
            }


            ConcurrentDictionary<int, IRegistryElement> effects;
            registar.TryGetValue(effectTypeId, out effects);

            result = new NativeHashMap<int, ELEMENT>(effects.Count, Allocator.Persistent);

            var effectEnumerator = effects.GetEnumerator();

            // Copy all effect to the result NativeHashMap.
            while (effectEnumerator.MoveNext())
            {
                var current = effectEnumerator.Current;

                result.TryAdd(current.Key, (ELEMENT)current.Value);
            }


        }

        public void SubscribeToElementRegisteredEvent(EventHandler method)
        {
            NewElementRegistered += method;
        }
        public void UnsubscribeToElementRegisteredEvent(EventHandler method)
        {
            NewElementRegistered -= method;
        }

 
    }
}

