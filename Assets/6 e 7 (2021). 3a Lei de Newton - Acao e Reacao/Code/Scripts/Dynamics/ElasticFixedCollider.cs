using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private float restitutionCoefficient;

        protected override void AffectCollider(Collider otherCollider)
        {
            otherCollider.RigidBody.Velocity *= -restitutionCoefficient;
            var pos = otherCollider.transform.position;
            pos.y = collider.PosVertex.y + otherCollider.Size.y/2;
            otherCollider.transform.position = pos;
        }
    }
}