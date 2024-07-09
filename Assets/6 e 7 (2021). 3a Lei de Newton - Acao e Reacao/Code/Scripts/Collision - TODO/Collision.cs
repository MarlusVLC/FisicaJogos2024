using System;
using UnityEngine;

namespace _6.AcaoReacao
{
    public static class Collision
    {
        public static bool DoOverlap(Collider a, Collider b, bool useMomentumConservation = false)
        {
            var doOverlap = false;
            var areCollidersCorrectlySet = false;
            if (a is ProtoBoxCollider && b is ProtoBoxCollider)
            {
                doOverlap = DoOverlap((ProtoBoxCollider)a, (ProtoBoxCollider)b);
                areCollidersCorrectlySet = true;
            }
            else if (a is ProtoSphericalCollider && b is ProtoSphericalCollider)
            {
                doOverlap = DoOverlap((ProtoSphericalCollider)a, (ProtoSphericalCollider)b);
                areCollidersCorrectlySet = true;
            }
            else if (a is ProtoBoxCollider && b is ProtoSphericalCollider)
            {
                return false;
                // doOverlap = DoOverlap((ProtoBoxCollider)a, (ProtoSphericalCollider)b);
                // areCollidersCorrectlySet = true;
            }
            else
            {
                return false;
                // doOverlap = DoOverlap((ProtoSphericalCollider)a, (ProtoBoxCollider)b);
                // areCollidersCorrectlySet = true;
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

        public static bool DoOverlap(ProtoBoxCollider a, ProtoBoxCollider b)
        {
            return
                a.NegVertex.x <= b.PosVertex.x &&
                a.PosVertex.x >= b.NegVertex.x &&
                a.NegVertex.y <= b.PosVertex.y &&
                a.PosVertex.y >= b.NegVertex.y &&
                a.NegVertex.z <= b.PosVertex.z &&
                b.PosVertex.z >= b.NegVertex.z;
        }

        //TODO(Marlus) to be built later
        // public static bool DoOverlap(ProtoSphericalCollider sphere, ProtoBoxCollider box)
        // {
            
        // }
    }
}