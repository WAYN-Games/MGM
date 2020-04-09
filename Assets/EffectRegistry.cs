using System;
using System.Collections.Concurrent;
using Unity.Collections;
using UnityEngine;

namespace Wayn.Mgm.Effects
{
    public class EffectRegistry
    {
        /// <summary>
        /// Tree to store all effect by Type and Version.
        /// </summary>
        private ConcurrentDictionary<ulong, ConcurrentDictionary<int, IEffect>> registar;

        private EffectRegistry()
        {  
            // Using Concurrent version of the dictionary to ovoid problem of reading while writing event though it should not happen.
            this.registar = new ConcurrentDictionary<ulong, ConcurrentDictionary<int, IEffect>>();
        }
        private static readonly Lazy<EffectRegistry> Instancelock =
                    new Lazy<EffectRegistry>(() => new EffectRegistry());


        // Declare the delegate (if using non-generic pattern).
        public delegate void NewEffectRegisteredHandler();

        // Declare the event.
        public event NewEffectRegisteredHandler NewEffectRegisteredEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseSampleEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            NewEffectRegisteredEvent?.Invoke();
        }

        public static EffectRegistry Instance
        {
            get
            {
                return Instancelock.Value;
            }
          
        }

        /// <summary>
        /// Add an effect to the registery and return its EffectReference.
        /// Effect taht have the same type and values are registered only once under the same EffectReference.
        /// </summary>
        /// <param name="effect">The effect to register.</param>
        /// <returns><see cref="EffectReference"/> A unique reference to that version of the effect.</returns>
        public EffectReference AddEffect(Wayn.Mgm.Effects.IEffect effect)
        {
            ulong effectTypeId = EffectReference.GetTypeId(effect.GetType());
            int effectVersionId = EffectReference.GetEffectInstanceId(effect);



            // Lazy initialzation
            if (!registar.ContainsKey(effectTypeId))
            {
                // Using Concurrent version of the dictionary to ovoid problem of reading while writing event though it should not happen.
                registar[effectTypeId] = new ConcurrentDictionary<int, IEffect>();
            }

            // Avoid duplicates
            if (!registar[effectTypeId].ContainsKey(effectVersionId))
            {
                registar[effectTypeId].TryAdd(effectVersionId, effect);
            }
            RaiseSampleEvent();
            return new EffectReference() { TypeId = effectTypeId, VersionId = effectVersionId };
        }



        /// <summary>
        /// Extract a copy of all effect of a given type referenced by VersionId.
        /// Caching the result is advised.
        /// </summary>
        /// <typeparam name="T">The type of effect to extract.</typeparam>
        /// <param name="type">The type of effect to extract.</param>
        /// <param name="result">The NativeHashmap to copy the effects to.</param>
        public void GetRegisteredEffects<T>(ref NativeHashMap<int, T> result) where T : struct, IEffect
        {
            ulong effectTypeId = EffectReference.GetTypeId(typeof(T));
             // If no effect are registered for that type return.
            if (!registar.ContainsKey(effectTypeId))
            {
                result = new NativeHashMap<int, T>(0, Allocator.Persistent);
                return;
            }
          

            ConcurrentDictionary<int, IEffect> effects;
            registar.TryGetValue(effectTypeId,out effects);

            result = new NativeHashMap<int, T>(effects.Count, Allocator.Persistent);

            var effectEnumerator = effects.GetEnumerator();

            // Copy all effect to the result NativeHashMap.
            while (effectEnumerator.MoveNext())
            {
                var current = effectEnumerator.Current;
                
                result.TryAdd(current.Key, (T)current.Value);
            }


        }

    }
}
