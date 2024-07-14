using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private float restitutionCoefficient;
        [SerializeField] private Vector3 afterCollisionDirection;

        protected override void AffectCollider(Collider otherCollider)
        {
            var newPosition = otherCollider.transform.position;
            var newVelocity = otherCollider.RigidBody.Velocity;
            if (afterCollisionDirection.x != 0)
            {
                newPosition.x = (collider.Center.x + (collider.Size/2).x * afterCollisionDirection.x) + otherCollider.Size.x / 2 * afterCollisionDirection.x;
                newVelocity.x *= (-1)*restitutionCoefficient;
            }
            if (afterCollisionDirection.y != 0)
            {
                newPosition.y = (collider.Center.y + (collider.Size/2).y * afterCollisionDirection.y) + otherCollider.Size.y / 2 * afterCollisionDirection.y;
                newVelocity.y *= (-1)*restitutionCoefficient;
            }
            otherCollider.transform.position = newPosition;
            otherCollider.RigidBody.Velocity = newVelocity;
        }
    }
}