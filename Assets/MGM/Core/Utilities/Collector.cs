using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using static Unity.Physics.Math;
using UnityEngine.Assertions;

namespace MGM
{
    public struct ClosestHitWithIgnoreCollector : ICollector<Unity.Physics.RaycastHit>
    {
        public Entity EntityToIgnore;
        public NativeSlice<RigidBody> Bodies;

        public bool EarlyOutOnFirstHit => false;
        public float MaxFraction { get; private set; }
        public int NumHits { get; private set; }

        private RaycastHit m_OldHit;
        private RaycastHit m_ClosestHit;
        public RaycastHit Hit => m_ClosestHit;

        public ClosestHitWithIgnoreCollector(float maxFraction, NativeSlice<RigidBody> rigidBodies, Entity entityToIgnore)
        {
            m_OldHit = default(RaycastHit);
            m_ClosestHit = default(RaycastHit);
            MaxFraction = maxFraction;
            NumHits = 0;
            Bodies = rigidBodies;
            EntityToIgnore = entityToIgnore;
        }

        #region ICollector

        public bool AddHit(RaycastHit hit)
        {
            Assert.IsTrue(hit.Fraction < MaxFraction);

            MaxFraction = hit.Fraction;
            m_OldHit = m_ClosestHit;
            m_ClosestHit = hit;
            NumHits = 1;
            return true;

        }

        void CheckIsAcceptable(float oldFraction)
        {
            var isAcceptable = Bodies[m_ClosestHit.RigidBodyIndex].Entity != EntityToIgnore;
            if (!isAcceptable)
            {
                m_ClosestHit = m_OldHit;
                NumHits = 0;
                MaxFraction = oldFraction;
                m_OldHit = default;
            }
        }

        public void TransformNewHits(int oldNumHits, float oldFraction, MTransform transform, uint numSubKeyBits, uint subKey)
        {
            m_ClosestHit.Transform(transform, numSubKeyBits, subKey);
        }

        public void TransformNewHits(int oldNumHits, float oldFraction, MTransform transform, int rigidBodyIndex)
        {
            m_ClosestHit.Transform(transform, rigidBodyIndex);
            CheckIsAcceptable(oldFraction);
        }
        #endregion
    }
}
