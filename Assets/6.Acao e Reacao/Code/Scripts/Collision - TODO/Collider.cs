using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(RigidBody))]
    public abstract class Collider : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool alwaysDrawGizmo;
        [SerializeField] public UnityEvent<Collider> onCollision;

        private Collider[] nearbyColliders;

        public RigidBody RigidBody { get; private set; }
        public int SystemID { get; private set; } = -1;

        public abstract Vector3 Size { get; }
        public Vector3 Center => transform.position + offset;
        public Vector3 PosVertex => Center + Size/2;
        public Vector3 NegVertex => Center - Size/2;
        // protected Collider[] NearbyColliders => nearbyColliders;

        protected void Start()
        {
            RigidBody = GetComponent<RigidBody>();
        }

        protected void OnEnable()
        {
            SystemID = CollisionManager.Instance.AddCollider(this);

            if (CollisionManager.Instance.HaltMovementOnCollision)
            {
                onCollision.AddListener(_ => RigidBody.HaltMovement());
                return;
            }
            if (CollisionManager.Instance.UseElasticCollision)
            {
                onCollision.AddListener(SetVelocityAfterElasticCollision);
                return;
            }
        }

        protected void OnDisable()
        {
            onCollision.RemoveAllListeners();
            
            if (CollisionManager.hasInstance == false)
            {
                return;
            }
            CollisionManager.Instance.RemoveCollider(this);
        }

        protected abstract void DrawShape();

        protected void OnDrawGizmos()
        {
            if (alwaysDrawGizmo == false)
            {
                return;
            }
            DrawShape();
        }

        protected void OnDrawGizmosSelected()
        {
            DrawShape();
        }

        private void SetVelocityAfterElasticCollision(Collider other)
        {
            var rbA = RigidBody;
            var rbB = other.RigidBody;
            rbA.Velocity = rbA.Momentum + rbB.Momentum + rbB.Mass * (rbB.Velocity - rbA.Velocity);
            rbA.Velocity /= rbA.Mass + rbB.Mass;
            Debug.Log($"Resultant velocity of {rbA.name}'s RigidBody after elastic impact = {rbA.Velocity}");
        }

        protected void FixedUpdate()
        {
            // if (CollisionManager.Instance.GetNearbyColliders(systemID, trueScale.magnitude, out nearbyColliders) <= 0)
            // {
            //     return;
            // }
            // NarrowPhaseCheck();
        }

        // protected abstract void NarrowPhaseCheck();
    }
}