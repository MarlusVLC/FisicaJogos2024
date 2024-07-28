using UnityEngine;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(ProtoBoxBoundingShape))]
    public class FluidField : EnvironmentCollider
    {
        [SerializeField] private float fluidDensity;

        private const float contactArea = 0.625f;

        protected override void AffectCollider(BoundingShape otherBoundingShape)
        {
            var rb = otherBoundingShape.RigidBody;
            rb.AddOppositeForce(0.5f * rb.Velocity.sqrMagnitude * fluidDensity * otherBoundingShape.Material.Friction * contactArea) ;
        }
    }
}