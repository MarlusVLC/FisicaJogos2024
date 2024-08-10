using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _6.AcaoReacao
{
    //TODO(Marlus) Try applying an UniformGrid algorithm
    public class CollisionManager : Singleton<CollisionManager>
    {
        [SerializeField] private float VelocityThreshold;
        [SerializeField] private float defaultDetectionRadius;
        [SerializeField] private bool useElasticCollision;
        [SerializeField] private bool haltMovementOnCollision;
        
        private readonly Dictionary<int, BoundingShape> colliders = new();
        private int keyCount = 0;
        
        public bool UseElasticCollision => useElasticCollision;
        public bool HaltMovementOnCollision => haltMovementOnCollision;
        public IReadOnlyDictionary<int, BoundingShape> Colliders => colliders;

        //BROAD PHASE
            //Brute Force <- Utilizada atualmente
            //Sweep and Prune
            //Uniform Grid

        private void FixedUpdate()
        {
            BroadPhaseCheck();
        }

        public int AddCollider(BoundingShape boundingShape)
        {
            if (boundingShape.SystemID >= 0 && colliders.ContainsKey(boundingShape.SystemID) == false)
            {
                colliders.Add(boundingShape.SystemID, boundingShape);
                return boundingShape.SystemID;
            }
            colliders.Add(keyCount, boundingShape);
            return keyCount++;
        }

        public void RemoveCollider(BoundingShape boundingShape)
        {
            if (boundingShape.SystemID < 0)
            {
                throw new Exception($"{boundingShape.name} haven't been added to the collision manager!");
            }
            if (colliders.ContainsKey(boundingShape.SystemID) == false)
            {
                throw new Exception($"{boundingShape.name} couldn't be found on the collision manager!");
            }
            Debug.Log($"{boundingShape.name} have been REMOVED and it had the following SystemID = {boundingShape.SystemID}");
            // collider.onCollision.AddListener(DebugCollision);
            colliders.Remove(boundingShape.SystemID);
        }

        private void BroadPhaseCheck()
        {
            for (var i = 0; i < keyCount; i++) 
            {
                for (var j = i + 1; j < keyCount; j++)
                {

                    if ((colliders.ContainsKey(i) && colliders.ContainsKey(j)) == false)
                    {
                        continue;
                    }

                    var a = colliders[i];
                    var b = colliders[j];

                    if (Collision.DoOverlap(a, b))
                    {
                        a.onCollisionIn.Invoke(b);
                        b.onCollisionIn.Invoke(a);
                        if (a.isLogicalField == false && b.isLogicalField == false)
                        {
                            Debug.Log($"{a.name} is colliding with {b.name}");
                            CollisionResponse(a, b);
                        }
                    }
                    
            //         if (Collision.DoOverlap(a, b))
            //         {
            //             if (a.WasIntersecting && b.WasIntersecting)
            //             {
            //                 a.onCollisionStay.Invoke(b);
            //                 b.onCollisionStay.Invoke(a);
            //             }
            //         }
            //         // else
            //         // {
            //         //     a.onCollisionOut.Invoke(b);
            //         //     b.onCollisionOut.Invoke(a);
            //         // }
            //         else
            //         {
            //             if (a.RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold &&
            //                 b.RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold)
            //             {
            //                 continue;
            //             }
            //
            //             if (!Collision.DoOverlap(a, b))
            //             {
            //                 continue;
            //             }
            //             a.onCollisionIn.Invoke(b);
            //             b.onCollisionIn.Invoke(a);
            //         }
                }
            }
        }

        private void CollisionResponse(BoundingShape colliderA, BoundingShape colliderB)
        {
            var rbA = colliderA.RigidBody;
            var rbB = colliderB.RigidBody;

            Vector3 normal = (rbB.transform.position - rbB.transform.position).normalized;

            Vector3 relativeVelocity = rbB.Velocity - rbA.Velocity;
            float velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);
            Debug.Log("Velocity along normal = " + velocityAlongNormal);

            if (velocityAlongNormal > 0)
                return;

            // float restitution = (colliderA.Material.Bounciness + colliderB.Material.Bounciness) / 2;
            float restitution = 0.9f;

            float impulseScalar = (-(1 + restitution) * velocityAlongNormal) / (1 /rbA.Mass + 1 / rbB.Mass);

            Vector3 impulse = impulseScalar * normal;
            rbA.Velocity -= (1 / rbA.Mass) * impulse;
            rbB.Velocity += (1 / rbB.Mass) * impulse;
        
            // rbA.Velocity = rbA.Momentum + rbB.Momentum + rbB.Mass * (rbB.Velocity - rbA.Velocity);
            // rbA.Velocity /= rbA.Mass + rbB.Mass;
            Debug.Log($"Resultant velocity of {rbA.name}'s RigidBody after elastic impact = {rbA.Velocity}");
            Debug.Log($"Resultant velocity of {rbB.name}'s RigidBody after elastic impact = {rbB.Velocity}");
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, defaultDetectionRadius);
        }

        public int GetCollidersCountInScene() => colliders.Count;
    }
}