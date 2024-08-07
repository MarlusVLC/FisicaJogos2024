﻿using UnityEngine;

namespace _6.AcaoReacao
{
    public abstract class EnvironmentCollider : MonoBehaviour
    {
        protected ProtoBoxCollider collider;

        private void Awake()
        {
            collider = GetComponent<ProtoBoxCollider>();
        }
        
        protected void FixedUpdate()
        {
            foreach (var otherCollider in CollisionManager.GetValidBoxColliders())
            {
                if (otherCollider.gameObject.activeSelf == false)
                    continue;
                if (!Collision.DoOverlap(collider, otherCollider)) 
                    continue;
               AffectCollider(otherCollider);
            }
        }

        protected abstract void AffectCollider(Collider otherCollider);
    }
}
