using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public class RigidBodyCanceler : MonoBehaviour
    {
        [SerializeField] private ProtoBoxCollider projectile;
        private ProtoBoxCollider collider;

        private void Awake()
        {
            collider = GetComponent<ProtoBoxCollider>();
        }

        private void FixedUpdate()
        {
            if (Collision.DoOverlap(projectile, collider))
            {
                projectile.RigidBody.HaltMovement();
            }
        }
    }
}

