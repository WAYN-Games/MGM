using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

namespace Wayn.Mgm.Combat.Effects
{
    /// <summary>
    /// This effect add the Amount the the Target entity's Health
    /// </summary>
    [Serializable]
    public struct KillLogEffect : IEffect
    {
        [HideInInspector] // Empty IEffect struct are not supported.
        public bool dummy;
    }

    [UpdateBefore(typeof(DestroyEntityHirearchyEffectConsumerSystem))]
    [UpdateBefore(typeof(DisableEntityHierarchyEffectConsumerSystem))]
    public class KillLogEffectConsumer : EffectConsumerSystem<KillLogEffect>
    {
        protected override JobHandle ScheduleJob(
            in NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator,
            in NativeHashMap<int, KillLogEffect> RegisteredEffects)
        {
            return new ConsumerJob()
            {
                EffectCommandEnumerator = EffectCommandEnumerator,
                EntityNames = GetComponentDataFromEntity<EntityName>(true)
            }.Schedule(Dependency);
        }


        public struct ConsumerJob : IJob
        {
            [ReadOnly]
            public NativeMultiHashMap<MapKey, EffectCommand>.Enumerator EffectCommandEnumerator;
            [ReadOnly]
            public ComponentDataFromEntity<EntityName> EntityNames;

            public void Execute()
            {
                while (EffectCommandEnumerator.MoveNext())
                {
                    string n1 = "";
                    string n2 = "";
                    if (EntityNames.Exists(EffectCommandEnumerator.Current.Emitter)) { 
                        ref BlobString e1 = ref EntityNames[EffectCommandEnumerator.Current.Emitter].Value.Value.str;
                        n1 = e1.ToString();
                    }
                    if (EntityNames.Exists(EffectCommandEnumerator.Current.Target))
                    {
                        ref BlobString e2 = ref EntityNames[EffectCommandEnumerator.Current.Target].Value.Value.str;
                        n2 = e2.ToString();
                    }
                    Debug.Log($"{n1} killed {n2}.");
        
                }
            }


        }
    }
}