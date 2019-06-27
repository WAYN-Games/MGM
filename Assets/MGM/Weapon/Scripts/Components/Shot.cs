using MGM.Common;
using System;
using Unity.Entities;
using Unity.Transforms;

namespace MGM.Weapon
{
    [Serializable]
    public struct ECS_VFX : IComponentData
    {
        public int vfxIndex;
        public LocalToWorld ltw;
        public bool play;

    }

        /// <summary>
        /// Minimal data required by the shot mechanic.
        /// </summary>
        [Serializable]
    public struct Shot : IComponentData
    {
        public Entity Projectile; // The projectile to be shot.
        public ECS_VFX MuzzleFlashVFX;
        public float Speed; // The speed at which the projectile will move
        public Trigger Trigger; // The trigger data to know when to shoot.

        public bool IsTriggered(ref Shot shot, float DeltaTime)
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
}
