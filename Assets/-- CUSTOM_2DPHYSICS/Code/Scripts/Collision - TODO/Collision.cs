using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public static class Collision
    {
        public static bool DoOverlap(BoundingShape a, BoundingShape b)
        {
            if (ShouldCollisionBeIgnored(a, b))
            {
                return false;
            }
            
            var doOverlap = false;
            var areCollidersCorrectlySet = false;
            switch (a)
            {
                case ProtoBoxBoundingShape boxA when b is ProtoBoxBoundingShape boxB:
                    doOverlap = DoOverlap(boxA, boxB);
                    areCollidersCorrectlySet = true;
                    break;
                case ProtoSphericalBoundingShape sphereA when b is ProtoSphericalBoundingShape sphereB:
                    doOverlap = DoOverlap(sphereA, sphereB);
                    areCollidersCorrectlySet = true;
                    break;
                case ProtoBoxBoundingShape box when b is ProtoSphericalBoundingShape sphere:
                    doOverlap = DoOverlap(sphere, box);
                    areCollidersCorrectlySet = true;
                    break;
                default:
                    doOverlap = DoOverlap((ProtoSphericalBoundingShape)a, (ProtoBoxBoundingShape)b);
                    areCollidersCorrectlySet = true;
                    break;
            }

            if (!areCollidersCorrectlySet)
            {
                throw new Exception(
                    "Colliders must be sphere or box!");
            }

            if (doOverlap)
            {
                Debug.Log($"{a.name} and {b.name} are colliding!");
            }
            
            return doOverlap;
        }
        
        private static bool DoOverlap(ProtoSphericalBoundingShape a, ProtoSphericalBoundingShape b)
        {
            var sqrDistance = Vector3.Magnitude(b.Center - a.Center);
            var sqrRadiiSum = a.Size.magnitude + b.Size.magnitude;
            return sqrDistance <= sqrRadiiSum;
        }

        private static bool DoOverlap(ProtoBoxBoundingShape a, ProtoBoxBoundingShape b, float threshold = 0f)
        {
            return
                a.NegVertex.x-threshold <= b.PosVertex.x &&
                a.PosVertex.x+threshold >= b.NegVertex.x &&
                a.NegVertex.y-threshold <= b.PosVertex.y &&
                a.PosVertex.y+threshold >= b.NegVertex.y &&
                a.NegVertex.z-threshold <= b.PosVertex.z &&
                b.PosVertex.z+threshold >= b.NegVertex.z;
        }

        private static bool DoOverlap(ProtoSphericalBoundingShape sphere, ProtoBoxBoundingShape box)
        {
            // Acha o ponto mais perto na caixa em relação ao centro da esfera
            Vector3 closestPoint = new Vector3(
                Math.Max(box.NegVertex.x, Math.Min(sphere.Center.x, box.PosVertex.x)),
                Math.Max(box.NegVertex.y, Math.Min(sphere.Center.y, box.PosVertex.y)),
                Math.Max(box.NegVertex.z, Math.Min(sphere.Center.z, box.PosVertex.z))
            );
            
            // Calcula a distância entre o ponto mais próximo e o centro da esfera
            float sqrDistance = (closestPoint - sphere.Center).sqrMagnitude;
            float radoisSquared = sphere.Size.sqrMagnitude;

            return sqrDistance <= radoisSquared;
        }
        
        public static bool ShouldCollisionBeIgnored(BoundingShape a, BoundingShape b) =>
            (a.gameObject.Equals(b.gameObject))  // Garante, de maneira bruta, a criação de um grupo de colisão
            || (a.isActiveAndEnabled && b.isActiveAndEnabled) == false 
            || Physics2D.GetIgnoreLayerCollision(a.gameObject.layer, b.gameObject.layer);
        
        public static float GetResultantBounciness(BoundingShape a, BoundingShape b) =>
            CustomPhysicsMaterial.GetResultantBounciness(a.Material, b.Material);
    }
}