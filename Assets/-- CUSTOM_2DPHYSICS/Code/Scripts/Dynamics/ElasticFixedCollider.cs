using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private Vector3 afterCollisionDirection;
        [SerializeField] private float frictionThreshold;
        [SerializeField] private bool useFriction;

        protected override void AffectCollider(BoundingShape otherBoundingShape)
        {
            var newPosition = otherBoundingShape.transform.position;
            var newVelocity = otherBoundingShape.RigidBody.Velocity;
            if (afterCollisionDirection.x != 0)
            {
                newPosition.x = (boundingShape.Center.x + (boundingShape.Size/2).x * afterCollisionDirection.x) + otherBoundingShape.Size.x / 2 * afterCollisionDirection.x;
                newVelocity.x *= (-1) *
                                 CustomPhysicsMaterial.GetResultantBounciness(boundingShape.Material, otherBoundingShape.Material);
            }
            if (afterCollisionDirection.y != 0)
            {
                newPosition.y = (boundingShape.Center.y + (boundingShape.Size/2).y * afterCollisionDirection.y) + otherBoundingShape.Size.y / 2 * afterCollisionDirection.y;
                newVelocity.y *= (-1) *
                                 CustomPhysicsMaterial.GetResultantBounciness(boundingShape.Material, otherBoundingShape.Material);
            } 
            otherBoundingShape.RigidBody.Velocity = newVelocity;
            otherBoundingShape.transform.position = newPosition;

            if (!Collision.DoOverlap(boundingShape, otherBoundingShape))
                return;
            ApplyFriction(otherBoundingShape);
        }

        private void ApplyFriction(BoundingShape otherBoundingShape)
        {
            if (!useFriction)
                return;
            var friction = CustomPhysicsMaterial.GetResultantFriction(boundingShape.Material, otherBoundingShape.Material);
            var rb = otherBoundingShape.RigidBody;
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
            otherBoundingShape.RigidBody.AddOppositeForce(friction * normalForce);
        }
    }
}