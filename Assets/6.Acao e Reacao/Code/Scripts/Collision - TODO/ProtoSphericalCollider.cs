using UnityEngine;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    [Tooltip("Still in construction, not working correctly")]
    public class ProtoSphericalCollider : Collider
    {
        [field: SerializeField] public float CollisionRadius { get; private set; } = 1;

        public override Vector3 Size => transform.localScale * CollisionRadius;

        // protected override void NarrowPhaseCheck()
        // {
        //     for (var i = 0; i < NearbyColliders.Length; i++)
        //     {
        //         if (NearbyColliders[i] == null)
        //         {
        //             break;
        //         }
        //         onCollision.Invoke(NearbyColliders[i]);
        //     }
        // }

        protected override void DrawShape()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Size.magnitude);
        }
    }
}