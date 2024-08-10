using System;
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
        
        private void OnEnable()
        {
            boundingShape.onCollisionIn.AddListener(AffectCollider);
        }

        protected abstract void AffectCollider(BoundingShape otherBoundingShape);
    }
}
