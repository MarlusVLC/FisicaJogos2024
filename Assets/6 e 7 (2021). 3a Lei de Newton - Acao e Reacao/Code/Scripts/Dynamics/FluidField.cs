using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(ProtoBoxCollider))]
    public class FluidField : EnvironmentCollider
    {
        [SerializeField] private float fluidDensity;

        private const float contactArea = 0.625f;

        protected override void AffectCollider(Collider otherCollider)
        {
            var rb = otherCollider.RigidBody;
            rb.AddResistanceForce(0.5f * rb.Velocity.sqrMagnitude * fluidDensity * otherCollider.DragCoefficient * contactArea) ;
        }
    }
}