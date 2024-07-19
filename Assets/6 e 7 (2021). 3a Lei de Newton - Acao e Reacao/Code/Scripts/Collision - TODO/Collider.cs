using System;
using System.Collections.Generic;
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
        [Min(0)][SerializeField] private float dragCoefficient = 0.45f;
        [SerializeField] private CustomPhysicsMaterial material;
        [SerializeField] public UnityEvent<Collider> onCollisionIn;
        [SerializeField] public UnityEvent<Collider> onCollisionStay;
        [SerializeField] public UnityEvent<Collider> onCollisionOut;

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
        private List<Collider> intersectingColliders;
        // protected Collider[] NearbyColliders => nearbyColliders;

        protected void Start()
        {
            RigidBody = GetComponent<RigidBody>();
        }

        protected void OnEnable()
        {
            SystemID = CollisionManager.Instance.AddCollider(this);
            intersectingColliders = new( CollisionManager.Instance.GetCollidersCountInScene());
            SetIntersectionEvents();
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
            mainGizmoColor = WasIntersecting ? Color.red : Color.green;
            DrawShape();
        }

        protected void OnDrawGizmosSelected()
        {
            mainGizmoColor = WasIntersecting ? Color.red : Color.green;
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