using System;
using Unity.Entities;
using UnityEngine;

namespace MGM {
    [Serializable]
    public struct MouvementCapabilityParameters : IComponentData
    {
        public float Speed;
        public bool ShouldFaceForward;
        public float MovementInertia;
        public Vector2 direction;
    }
}
