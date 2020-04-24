using System;
using System.Collections.Concurrent;
using Unity.Collections;
using Unity.Entities;

namespace Wayn.Mgm.Events.Registry
{
    public interface IRegistry<ELEMENT>
             where ELEMENT : IRegistryElement
    {
        RegistryReference AddEffect(ELEMENT element);
    }
    public abstract class Registry<T, R> : IRegistry<R>
        where T : Registry<T, R>
        where R : IRegistryElement
    {
        private static readonly Lazy<T> Lazy =
            new Lazy<T>(Init
        );
         
        private static T Init()
        {
            T i = Activator.CreateInstance(typeof(T), true) as T;
            i.registar = new ConcurrentDictionary<ulong, ConcurrentDictionary<int, R>>();
            return i;
        }


        public static T Instance => Lazy.Value;
        /// <summary>
        /// Tree to store all effect by Type and Version.
        /// </summary>
        protected ConcurrentDictionary<ulong, ConcurrentDictionary<int, R>> registar;


        /// <summary>
        /// Add an effect to the registery and return its EffectReference.
        /// Effect taht have the same type and values are registered only once under the same EffectReference.
        /// </summary>
        /// <param name="effect">The effect to register.</param>
        /// <returns><see cref="RegistryReference"/> A unique reference to that version of the effect.</returns>
        public RegistryReference AddEffect(R effect)
        {
            ulong effectTypeId = RegistryReference.GetTypeId(effect.GetType());
            int effectVersionId = RegistryReference.GetEffectInstanceId(effect);
          

            // Lazy initialzation
            if (!registar.ContainsKey(effectTypeId))
            {
                // Using Concurrent version of the dictionary to ovoid problem of reading while writing event though it should not happen.
                registar[effectTypeId] = new ConcurrentDictionary<int, R>();
            }

            // Avoid duplicates
            if (!registar[effectTypeId].ContainsKey(effectVersionId))
            {
                registar[effectTypeId].TryAdd(effectVersionId, effect);
            }
            OnNewElementRegistered();
            return new RegistryReference() { TypeId = effectTypeId, VersionId = effectVersionId };
        }

        protected abstract void OnNewElementRegistered();

        /// <summary>
        /// Extract a copy of all effect of a given type referenced by VersionId.
        /// Caching the result is advised.
        /// </summary>
        /// <typeparam name="T">The type of effect to extract.</typeparam>
        /// <param name="type">The type of effect to extract.</param>
        /// <param name="result">The NativeHashmap to copy the effects to.</param>
        #pragma warning disable CS0693
        public void GetRegisteredEffects<T>(ref NativeHashMap<int, T> result) where T : struct, R
        #pragma warning restore CS0693
        {
            ulong effectTypeId = RegistryReference.GetTypeId(typeof(T));
            // If no effect are registered for that type return.
            if (!registar.ContainsKey(effectTypeId))
            {
                result = new NativeHashMap<int, T>(0, Allocator.Persistent);
                return;
            }


            ConcurrentDictionary<int, R> effects;
            registar.TryGetValue(effectTypeId, out effects);

            result = new NativeHashMap<int, T>(effects.Count, Allocator.Persistent);

            var effectEnumerator = effects.GetEnumerator();

            // Copy all effect to the result NativeHashMap.
            while (effectEnumerator.MoveNext())
            {
                var current = effectEnumerator.Current;

                result.TryAdd(current.Key, (T)current.Value);
            }


        }

        protected Registry()
        {
        }

    }

    public interface IRegistryElement
    {

    }

    public class EffectRegistry : Registry<EffectRegistry, IEffect>
    {
        // This is mandatory to enfore the singleton.
        private EffectRegistry() { }

        // Declare the event and delegate.
        public delegate void NewEffectRegisteredHandler();
        public event NewEffectRegisteredHandler NewEffectRegisteredEvent;

        protected override void OnNewElementRegistered()
        {   
            NewEffectRegisteredEvent?.Invoke();
        }

  

    }
}

