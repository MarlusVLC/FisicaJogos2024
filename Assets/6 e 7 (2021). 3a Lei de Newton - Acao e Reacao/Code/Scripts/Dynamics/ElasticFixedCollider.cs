using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private Vector3 afterCollisionDirection;
        [SerializeField] private float frictionThreshold;
        [SerializeField] private bool useFriction;

        protected override void AffectCollider(Collider otherCollider)
        {
            var newPosition = otherCollider.transform.position;
            var newVelocity = otherCollider.RigidBody.Velocity;
            if (afterCollisionDirection.x != 0)
            {
                newPosition.x = (collider.Center.x + (collider.Size/2).x * afterCollisionDirection.x) + otherCollider.Size.x / 2 * afterCollisionDirection.x;
                newVelocity.x *= (-1) *
                                 CustomPhysicsMaterial.GetResultantBounciness(collider.Material, otherCollider.Material);
            }
            if (afterCollisionDirection.y != 0)
            {
                newPosition.y = (collider.Center.y + (collider.Size/2).y * afterCollisionDirection.y) + otherCollider.Size.y / 2 * afterCollisionDirection.y;
                newVelocity.y *= (-1) *
                                 CustomPhysicsMaterial.GetResultantBounciness(collider.Material, otherCollider.Material);
            }
            otherCollider.RigidBody.Velocity = newVelocity;
            otherCollider.transform.position = newPosition;

            if (!Collision.DoOverlap(collider, (ProtoBoxCollider)otherCollider, frictionThreshold))
                return;
            ApplyFriction(otherCollider);
        }

        private void ApplyFriction(Collider otherCollider)
        {
            if (!useFriction)
                return;
            var friction = CustomPhysicsMaterial.GetResultantFriction(collider.Material, otherCollider.Material);
            var rb = otherCollider.RigidBody;
            //TODO Perguntar para professor: O que fazer em SUPERFÍCIES ANGULADAs, e quando o MOVIMENTO do objeto é ANGULADO 
            //TODO checar fórmulas no Desmos
            float normalForce = rb.Weight.y;
            // if (afterCollisionDirection.x != 0)
            // {
            //     normalForce = rb.Momentum.x;
            // }
            // if (afterCollisionDirection.y != 0)
            // {
            //     normalForce = rb.Momentum.y;
            // }
            otherCollider.RigidBody.AddOppositeForce(friction * normalForce);
        }
    }
}