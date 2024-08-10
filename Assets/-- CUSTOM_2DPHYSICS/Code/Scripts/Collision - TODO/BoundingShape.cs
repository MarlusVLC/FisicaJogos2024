using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(RigidBody))]
    //AKA Collider
    public abstract class BoundingShape : MonoBehaviour
    {
        [FormerlySerializedAs("isField")] [SerializeField] public bool isLogicalField;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool alwaysDrawGizmo;
        [Min(0)][SerializeField] private float dragCoefficient = 0.45f;
        [SerializeField] private CustomPhysicsMaterial material;
        [SerializeField] public UnityEvent<BoundingShape> onCollisionIn;
        [SerializeField] public UnityEvent<BoundingShape> onCollisionStay;
        [SerializeField] public UnityEvent<BoundingShape> onCollisionOut;

        private List<BoundingShape> intersectingColliders;
        // private Collider[] nearbyColliders;
        protected Color mainGizmoColor;

        public RigidBody RigidBody { get; private set; }
        public int SystemID { get; private set; } = -1;
        public bool WasIntersecting {get; private set; }

        public float DragCoefficient => dragCoefficient;
        public CustomPhysicsMaterial Material => material;

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
            intersectingColliders = new( CollisionManager.Instance.GetCollidersCountInScene());
            // SetIntersectionEvents();
        }

        protected void OnDisable()
        {
            onCollisionIn.RemoveAllListeners();
            
            if (CollisionManager.HasInstance == false)
                return;
            CollisionManager.Instance.RemoveCollider(this);
        }

        protected void SetIntersectionEvents()
        {
            onCollisionIn.AddListener(_ => WasIntersecting = true);
            onCollisionIn.AddListener(intersectingColliders.Add);
            onCollisionOut.AddListener(_ => WasIntersecting = false);
            // onCollisionOut.AddListener();

            if (CollisionManager.Instance.HaltMovementOnCollision)
            {
                onCollisionIn.AddListener(_ => RigidBody.HaltMovement());
                return;
            }
            if (CollisionManager.Instance.UseElasticCollision)
            {
                onCollisionIn.AddListener(SetVelocityAfterElasticCollision);
                return;
            }
        }

        protected abstract void DrawShape();

        protected void OnDrawGizmos()
        {
            if (alwaysDrawGizmo == false)
            {
                return;
            }

            if (enabled == false)
            {
                return;
            }
            mainGizmoColor = WasIntersecting ? Color.red : Color.green;
            DrawShape();
        }

        protected void OnDrawGizmosSelected()
        {
            if (enabled == false)
            {
                return;
            }
            mainGizmoColor = WasIntersecting ? Color.red : Color.green;
            DrawShape();
        }

        private void SetVelocityAfterElasticCollision(BoundingShape other)
        {
            var rbA = RigidBody;
            var rbB = other.RigidBody;

            Vector3 normal = (rbB.transform.position - rbB.transform.position).normalized;

            Vector3 relativeVelocity = rbB.Velocity - rbA.Velocity;
            float velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);
            
            if (velocityAlongNormal > 0)
                return;

            float restitution = (Material.Bounciness + Material.Bounciness) / 2;

            float impulseScalar = (-(1 + restitution) * velocityAlongNormal) / (1 /rbA.Mass + 1 / rbB.Mass);

            Vector3 impulse = impulseScalar * normal;
            rbA.Velocity -= (1 / rbA.Mass) * impulse;
            rbB.Velocity += (1 / rbB.Mass) * impulse;
            
            // rbA.Velocity = rbA.Momentum + rbB.Momentum + rbB.Mass * (rbB.Velocity - rbA.Velocity);
            // rbA.Velocity /= rbA.Mass + rbB.Mass;
            Debug.Log($"Resultant velocity of {rbA.name}'s RigidBody after elastic impact = {rbA.Velocity}");
        }

        // protected void FixedUpdate()
        // {
        //     if (CollisionManager.Instance.GetNearbyColliders(systemID, trueScale.magnitude, out nearbyColliders) <= 0)
        //     {
        //         return;
        //     }
        //     NarrowPhaseCheck();
        // }

        // protected abstract void NarrowPhaseCheck();
    }
}