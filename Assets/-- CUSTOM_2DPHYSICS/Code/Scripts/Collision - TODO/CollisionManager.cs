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
            // collider.onCollisionIn.AddListener(DebugCollision);
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

        private void DebugCollision(BoundingShape boundingShape)
        {
            Debug.Log("A collision has happened with " + boundingShape.name);
        }

        // public int GetNearbyColliders(int id, float colliderDetectionRadius, out Collider[] nearbyColliders)
        // {
        //     nearbyColliders = new Collider[colliders.Count-1];
        //     var broadIndexer = 0;
        //     for (var i = 0; i < colliders.Count; i++)
        //     {
        //         var nearbyIndex = i - broadIndexer;
        //         if (i == id)
        //         {
        //             broadIndexer++;
        //             continue;
        //         }
        //         var sqrDetectionRadius = Mathf.Pow(defaultDetectionRadius + colliderDetectionRadius, 2);
        //         var sqrDistance =
        //             Vector3.SqrMagnitude(colliders[i].transform.position - colliders[id].transform.position);
        //         if (sqrDistance < sqrDetectionRadius)
        //         {
        //             nearbyColliders[nearbyIndex] = colliders[i];
        //         }
        //         else
        //         {
        //             broadIndexer++;
        //         }
        //     }
        //     return broadIndexer; 
        // }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, defaultDetectionRadius);
        }

        public int GetCollidersCountInScene() => colliders.Count;
    }
}