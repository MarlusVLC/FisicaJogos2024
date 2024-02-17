using UnityEngine;

namespace _6.AcaoReacao
{
    //TODO (Marlus) finish later
    public class ProtoSphericalCollider : Collider
    {
        [field: SerializeField] public float collisionScale { get; private set; } 
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, trueScale.magnitude);
        }

        //TODO (Marlus) Finish/correct implementation
        public override Vector3 trueScale
        {
            get => transform.localScale * collisionScale; 
        }
    }
}