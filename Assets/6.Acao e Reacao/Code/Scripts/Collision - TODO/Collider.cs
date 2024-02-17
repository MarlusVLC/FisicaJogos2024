using System;
using UnityEngine;
using UnityEngine.Events;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    [RequireComponent(typeof(RigidBody))]
    public abstract class Collider : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onCollision;
        
        private int systemID;
        private Collider[] nearbyColliders;

        public RigidBody RigidBody { get; private set; }
        //TODO(Marlus) improve conceptualization
        public abstract Vector3 trueScale { get; }

        protected void Start()
        {
            systemID = CollisionManager.Instance.AddCollider(this);
            RigidBody = GetComponent<RigidBody>();
        }

        //TODO(Marlus) Finish Implementation
        protected void FixedUpdate()
        {
            if (CollisionManager.Instance.GetNearbyColliders(systemID, trueScale.magnitude, out nearbyColliders) <= 0)
            {
                return;
            }
            //TODO check collision (geometric intersection) with nearby colliders
            //TODO apply energy and forces transactions and MOMENTUM CONSERVATION
        }
    }
}