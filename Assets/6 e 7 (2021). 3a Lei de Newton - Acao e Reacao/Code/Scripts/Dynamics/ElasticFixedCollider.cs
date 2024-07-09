using UnityEngine;

namespace _6.AcaoReacao
{
    public class ElasticFixedCollider : EnvironmentCollider
    {
        [SerializeField] private float restitutionCoefficient;
        // [SerializeField] private ProtoBoxCollider[] targets;
        // private ProtoBoxCollider collider;

        // private void Awake()
        // {
        //     collider = GetComponent<ProtoBoxCollider>();
        // }
        //
        // private void FixedUpdate()
        // {
        //     foreach (var target in targets)
        //     {
        //         if (Collision.DoOverlap(target, collider))
        //         {
        //             target.RigidBody.Velocity *= -restitutionCoefficient;
        //             var pos = target.transform.position;
        //             pos.y = collider.PosVertex.y + target.Size.y/2;
        //             target.transform.position = pos;
        //         }
        //     }
        //
        // }
        protected override void AffectCollider(Collider otherCollider)
        {
            otherCollider.RigidBody.Velocity *= -restitutionCoefficient;
            var pos = otherCollider.transform.position;
            pos.y = collider.PosVertex.y + otherCollider.Size.y/2;
            otherCollider.transform.position = pos;
        }
    }
}