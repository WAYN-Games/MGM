using MGM.Common;
using System;
using Unity.Entities;

namespace MGM.Weapon
{
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


    }
}
