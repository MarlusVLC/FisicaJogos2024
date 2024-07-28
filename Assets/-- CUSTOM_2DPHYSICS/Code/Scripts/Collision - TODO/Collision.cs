using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public static class Collision
    {
        //TODO: Levar em consideração movimento para confirmar colisão.
        public static bool DoOverlap(Collider a, Collider b, bool useMomentumConservation = false)
        {
            var doOverlap = false;
            var areCollidersCorrectlySet = false;
            if (a is ProtoBoxCollider boxA && b is ProtoBoxCollider boxB)
            {
                doOverlap = DoOverlap(boxA, boxB);
                areCollidersCorrectlySet = true;
            }
            else if (a is ProtoSphericalCollider sphereA && b is ProtoSphericalCollider sphereB)
            {
                doOverlap = DoOverlap(sphereA, sphereB);
                areCollidersCorrectlySet = true;
            }
            else if (a is ProtoBoxCollider box && b is ProtoSphericalCollider sphere)  
            {
                // return false;
                doOverlap = DoOverlap(sphere, box);
                areCollidersCorrectlySet = true;
            }
            else
            {
                // return false;
                doOverlap = DoOverlap((ProtoSphericalCollider)a, (ProtoBoxCollider)b);
                areCollidersCorrectlySet = true;
            }

            if (!areCollidersCorrectlySet)
            {
                throw new Exception(
                    "Colliders must be sphere or box!");
            }

            return doOverlap;
        }
        
        //TODO (Marlus) - Must be fixed
        public static bool DoOverlap(ProtoSphericalCollider a, ProtoSphericalCollider b)
        {
            var sqrDistance = Vector3.Magnitude(b.Center - a.Center);
            var sqrRadiiSum = a.Size.magnitude + b.Size.magnitude;
            return sqrDistance <= sqrRadiiSum;
        }

        public static bool DoOverlap(ProtoBoxCollider a, ProtoBoxCollider b, float threshold = 0f)
        {
            return
                a.NegVertex.x-threshold <= b.PosVertex.x &&
                a.PosVertex.x+threshold >= b.NegVertex.x &&
                a.NegVertex.y-threshold <= b.PosVertex.y &&
                a.PosVertex.y+threshold >= b.NegVertex.y &&
                a.NegVertex.z-threshold <= b.PosVertex.z &&
                b.PosVertex.z+threshold >= b.NegVertex.z;
        }

        public static float GetResultantBounciness(Collider a, Collider b) =>
            CustomPhysicsMaterial.GetResultantBounciness(a.Material, b.Material);

        public static bool DoOverlap(ProtoSphericalCollider sphere, ProtoBoxCollider box)
        {
            // Acha o ponto mais perto na caixa em relação ao centro da esfera
            Vector3 closestPoint = new Vector3(
                Math.Max(box.NegVertex.x, Math.Min(sphere.Center.x, box.PosVertex.x)),
                Math.Max(box.NegVertex.y, Math.Min(sphere.Center.y, box.PosVertex.y)),
                Math.Max(box.NegVertex.z, Math.Min(sphere.Center.z, box.PosVertex.z))
            );
            
            // Calcula a distância entre o ponto mais próximo e o centro da esfera
            float sqrDistance = (closestPoint = sphere.Center).sqrMagnitude;
            float radoisSquared = sphere.Size.sqrMagnitude;

            return sqrDistance <= radoisSquared;
        }
    }
}