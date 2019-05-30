using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MGM.Core
{

    [Serializable]
    [WriteGroup(typeof(Shot))]
    public struct GridShot : IComponentData
    {
        public int SizeX;
        public int SizeY;
        public int Density;
    }


    /// <summary>
    /// Give the capability to shoot a single bullet.
    /// </summary>
    public class GridShotCapability : BaseShootingCapability
    {
        [Header("GridShot Properties")]
        public int SizeX;
        public int SizeY;
        public int Density;

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Setup the minimal component required for a shot mechanic.
            base.SetUpCapabilityParameters(entity, dstManager, conversionSystem);

            
            // Add a magazine
            Magazine magazine = new Magazine()
            {
                CurrentCapacity = MagazineCapacity,
                MaxCapacity = MagazineCapacity
            };
            dstManager.AddComponentData(entity, magazine);

            // Add a tag component to know that we want to override the base shot system.
            GridShot gridShot = new GridShot()
            {
                SizeX = SizeX,
                SizeY = SizeY,
                Density  =Density
            };
            dstManager.AddComponentData(entity, gridShot);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color= Color.magenta;
            float xRange = (float)SizeX / 2;
            float yRange = (float)SizeY / 2;
            for (float x = -xRange; x < xRange; x++)
            {
                for (float y = -yRange; y < yRange; y++)
                {
                    Gizmos.DrawLine(transform.position, transform.position + (transform.forward * Density) + (transform.right * (x+.5f)) + (transform.up * (y+.5f)));
                }
            }

        }
    }

}
