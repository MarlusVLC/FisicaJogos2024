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
        
        private Dictionary<int, Collider> colliders = new();
        private int keyCount = 0;
        
        //BROAD PHASE
            //Brute Force <- Utilizada atualmente
            //Sweep and Prune
            //Uniform Grid

        private void FixedUpdate()
        {
            BroadPhaseCheck();
        }

        public static IEnumerable<ProtoBoxCollider> GetValidBoxColliders() =>
            Instance.colliders.Values
                .Where(c => c is ProtoBoxCollider && 
                            (c.RigidBody.isKinematic == false && c.RigidBody.isStatic == false))
                .Cast<ProtoBoxCollider>();

        public bool UseElasticCollision => useElasticCollision;
        public bool HaltMovementOnCollision => haltMovementOnCollision;

        public int AddCollider(Collider collider)
        {
            // collider.onCollisionIn.AddListener(DebugCollision);
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
                    
                    //Garante, de maneira bruta, a criação de um grupo de colisão
                    if (a.gameObject.Equals(b.gameObject))
                    {
                        continue;
                    }
                    
                    if ((a.isActiveAndEnabled && b.isActiveAndEnabled) == false)
                    {
                        continue;
                    }

                    if (Physics2D.GetIgnoreLayerCollision(a.gameObject.layer, b.gameObject.layer))
                    {
                        continue;
                    }
                    
                    if (Collision.DoOverlap(a, b))
                    {
                        if (a.WasIntersecting && b.WasIntersecting)
                        {
                            a.onCollisionStay.Invoke(b);
                            b.onCollisionStay.Invoke(a);
                        }
                    }
                    // else
                    // {
                    //     a.onCollisionOut.Invoke(b);
                    //     b.onCollisionOut.Invoke(a);
                    // }
                    else
                    {
                        if (a.RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold &&
                            b.RigidBody.Velocity.sqrMagnitude < VelocityThreshold * VelocityThreshold)
                        {
                            continue;
                        }

                        if (!Collision.DoOverlap(a, b))
                        {
                            continue;
                        }
                        a.onCollisionIn.Invoke(b);
                        b.onCollisionIn.Invoke(a);
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

        public int GetCollidersCountInScene() => colliders.Count;
    }
}