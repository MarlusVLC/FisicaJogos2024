using UnityEngine;

namespace _6.AcaoReacao
{
    public abstract class EnvironmentCollider : MonoBehaviour
    {
        protected BoundingShape boundingShape;

        private void Awake()
        {
            boundingShape = GetComponent<BoundingShape>();
        }
        
        protected void FixedUpdate()
        {
            foreach (var otherCollider in CollisionManager.Instance.Colliders.Values)
            {
                if (otherCollider.SystemID == boundingShape.SystemID)
                {
                    continue;
                }
                if (otherCollider.gameObject.activeSelf == false)
                    continue;
                if (!Collision.DoOverlap(boundingShape, otherCollider)) 
                    continue;
                AffectCollider(otherCollider);
            }
        }

        protected abstract void AffectCollider(BoundingShape otherBoundingShape);
    }
}
