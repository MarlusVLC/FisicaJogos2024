using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _6.AcaoReacao
{
    [RequireComponent(typeof(ProtoBoxCollider))]
    public class ProtoFluidField : EnvironmentCollider
    {
        [SerializeField] private float fluidDensity;
        // [SerializeField] private ProtoBoxCollider[] affectedObjects;
        // private ProtoBoxCollider collider;
        
        private const float contactArea = 0.625f;

        // private void Awake()
        // {
        //     collider = GetComponent<ProtoBoxCollider>();
        // }
        //
        // private void FixedUpdate()
        // {
        //     base.FixedUpdate();
        // }

        protected override void AffectCollider(Collider otherCollider)
        {
            var rb = otherCollider.RigidBody;
            // Debug.Log($"Force applied = " + 0.5f * rb.Velocity.sqrMagnitude * fluidDensity * otherCollider.DragCoefficient * contactArea);
            rb.AddResistanceForce(0.5f * rb.Velocity.sqrMagnitude * fluidDensity * otherCollider.DragCoefficient * contactArea) ;
        }
    }
}