using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public class RigidBodyCanceler : MonoBehaviour
    {
        [SerializeField] private ProtoBoxBoundingShape projectile;
        private ProtoBoxBoundingShape boundingShape;

        private void Awake()
        {
            boundingShape = GetComponent<ProtoBoxBoundingShape>();
        }

        private void FixedUpdate()
        {
            if (Collision.DoOverlap(projectile, boundingShape))
            {
                projectile.RigidBody.HaltMovement();
            }
        }
    }
}

