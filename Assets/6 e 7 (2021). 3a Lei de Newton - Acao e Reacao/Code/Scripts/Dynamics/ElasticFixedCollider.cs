using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private float restitutionCoefficient;

        protected override void AffectCollider(Collider otherCollider)
        {
            otherCollider.RigidBody.Velocity *= -restitutionCoefficient;
        }
    }
}