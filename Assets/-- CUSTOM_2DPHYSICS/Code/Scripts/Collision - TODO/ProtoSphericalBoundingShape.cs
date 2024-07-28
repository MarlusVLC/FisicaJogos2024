using UnityEngine;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    [Tooltip("Still in construction, not working correctly")]
    public class ProtoSphericalBoundingShape : BoundingShape
    {
        [field: SerializeField] public float CollisionRadius { get; private set; } = 1;

        public override Vector3 Size => transform.localScale * CollisionRadius;

        protected override void DrawShape()
        {
            Gizmos.color = mainGizmoColor;
            Gizmos.DrawWireSphere(transform.position, Size.magnitude);
        }
    }
}