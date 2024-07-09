using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public abstract class EnvironmentCollider : MonoBehaviour
    {
        protected ProtoBoxCollider collider;

        protected void Awake()
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

        abstract protected void AffectCollider(Collider otherCollider);
    }
}

