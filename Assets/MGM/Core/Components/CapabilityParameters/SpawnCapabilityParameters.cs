using System;
using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MGM {
    
    /// <summary>
    /// Minimal data required by the shot mechanic.
    /// </summary>
    [Serializable]
    public struct Shot : IComponentData
    {
        public Entity Projectile; // The projectile to be shot.
        public float Speed; // The speed at which the projectile will move
        public Trigger Trigger; // The trigger data to know when to shoot.

        public static bool IsTriggered(ref Shot shot, float DeltaTime)
        {
            // Increase the cool down count
            shot.Trigger.TimeSinceLastTrigger += DeltaTime;

            // Spawn object only when requested
            if (!shot.Trigger.IsTriggered) return false;

            // Reset the input trigger
            shot.Trigger.IsTriggered = false;

            // Shoot only if cooled down
            if (shot.Trigger.TimeSinceLastTrigger < shot.Trigger.CoolDown) return false;
            // Reset the cool down count
            shot.Trigger.TimeSinceLastTrigger = 0;
            return true;
        }
    }


    [Serializable]
    public struct Magazine : IComponentData
    {
   
        public int MaxCapacity;
        public int CurrentCapacity;

        public static bool IsMagazineEmpty(ref Magazine magazine)
        {
            // Allow for infinite magazine capacity
            if (magazine.CurrentCapacity == -1) return false;

            // Shoot only if projectile left.
            if (magazine.CurrentCapacity == 0) return true;
            // remove one projectile from the magazine.
            magazine.CurrentCapacity -= 1;
            return false;
        }
    }


    [Serializable]
    public struct Trigger : IComponentData
    {
        public bool IsTriggered; // True if the player want to trigger the action.
        public float CoolDown; // The time between two action trigger.
        public float TimeSinceLastTrigger; // The time elapsed since the last trigger was effectively triggered. 
    }

}
