using UnityEngine;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    [Tooltip("Still in construction, not working correctly")]
    public class ProtoSphericalBoundingShape : BoundingShape
    {
        [field: SerializeField] public float Radius { get; private set; } = 1;
        [SerializeField] private bool showDetailedGizmos;


        public override Vector3 Size => Vector3.one * (Radius * 2);
        public float Diameter => Radius * 2;

        protected override void DrawShape()
        {
            Gizmos.color = mainGizmoColor;
            Gizmos.DrawWireSphere(Center, Radius);
            
            // if (showDetailedGizmos == false)
            // {
            //     return;
            // }
            // Gizmos.color = Color.grey;
            // Gizmos.DrawWireSphere(PosVertex, 0.5f);
            // Gizmos.DrawWireSphere(NegVertex, 0.5f);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(new Vector3(PosVertex.x, Center.y, Center.z), 0.25f);
            // Gizmos.DrawWireSphere( new Vector3(Center.x, PosVertex.y, Center.z), 0.25f);
            // Gizmos.DrawWireSphere(new Vector3(Center.x, Center.y, PosVertex.z), 0.25f);
            // Gizmos.DrawWireSphere(new Vector3(NegVertex.x, Center.y, transform.position.z), 0.25f);
            // Gizmos.DrawWireSphere( new Vector3(Center.x, NegVertex.y, Center.z), 0.25f);
            // Gizmos.DrawWireSphere(new Vector3(Center.x, Center.y, NegVertex.z), 0.25f);
        }
    }
}