using UnityEngine;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private float restitutionCoefficient;
        [SerializeField] private Vector3 afterCollisionDirection;

        protected override void AffectCollider(Collider otherCollider)
        {
            otherCollider.RigidBody.Velocity *= -restitutionCoefficient;
            var newPosition = otherCollider.transform.position;
            if (afterCollisionDirection.x != 0)
            {
                newPosition.x = (collider.Center.x + (collider.Size/2).x * afterCollisionDirection.x) + otherCollider.Size.x / 2 * afterCollisionDirection.x;
            }
            if (afterCollisionDirection.y != 0)
            {
                newPosition.y = (collider.Center.y + (collider.Size/2).y * afterCollisionDirection.y) + otherCollider.Size.y / 2 * afterCollisionDirection.y;
            }
            otherCollider.transform.position = newPosition;
        }
    }
}