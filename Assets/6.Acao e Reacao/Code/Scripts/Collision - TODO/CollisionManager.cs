using System;
using System.Collections.Generic;
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
        
        private Dictionary<int, Collider> colliders = new();
        private int keyCount = 0;

        private void FixedUpdate()
        {
            NarrowPhaseCheck();
        }

        public bool UseElasticCollision => useElasticCollision;
        public bool HaltMovementOnCollision => haltMovementOnCollision;

        public int AddCollider(Collider collider)
        {
            collider.onCollision.AddListener(DebugCollision);
            if (collider.SystemID >= 0 && colliders.ContainsKey(collider.SystemID) == false)
            {
                colliders.Add(collider.SystemID, collider);
                return collider.SystemID;
            }
            colliders.Add(keyCount, collider);
            return keyCount++;
        }

        public void RemoveCollider(Collider collider)
        {
            if (collider.SystemID < 0)
            {
                throw new Exception($"{collider.name} haven't been added to the collision manager!");
            }
            if (colliders.ContainsKey(collider.SystemID) == false)
            {
                throw new Exception($"{collider.name} couldn't be found on the collision manager!");
            }
            Debug.Log($"{collider.name} have been REMOVED and it had the following SystemID = {collider.SystemID}");
            // collider.onCollision.AddListener(DebugCollision);
            colliders.Remove(collider.SystemID);
        }

        private void NarrowPhaseCheck()
        {
            for (var i = 0; i < keyCount; i++) 
            {
                for (var j = i + 1; j < keyCount; j++)
                {
                    if ((colliders.ContainsKey(i) && colliders.ContainsKey(j)) == false)
                    {
                        continue;
                    }

                    if (colliders[i].gameObject.Equals(colliders[j].gameObject))
                    {
                        continue;
                    }

                    if (colliders[i].RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold &&
                        colliders[j].RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold)
                    {
                        continue;
                    }
                    if (colliders[i].isActiveAndEnabled && colliders[j].isActiveAndEnabled)
                    {
                        Collision.DoOverlap(colliders[i], colliders[j], useElasticCollision);
                    }
                }
            }
        }

        private void DebugCollision(Collider collider)
        {
            Debug.Log("A collision has happened with " + collider.name);
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
    }
}