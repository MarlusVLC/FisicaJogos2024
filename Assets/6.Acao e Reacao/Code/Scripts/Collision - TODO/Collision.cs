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
            if (a is ProtoSphericalCollider && b is ProtoSphericalCollider)
            {
                doOverlap = DoOverlap((ProtoSphericalCollider)a, (ProtoSphericalCollider)b);
                areCollidersCorrectlySet = true;
            }

            if (!areCollidersCorrectlySet)
            {
                throw new Exception(
                    "Colliders must be sphere or box and both of the same shape. Different shape detection is still in development");
            }

            if (!doOverlap) return false;

            a.onCollision.Invoke(b);
            b.onCollision.Invoke(a);
            return true;

        }
        
        //TODO (Marlus) - Must be fixed
        public static bool DoOverlap(ProtoSphericalCollider a, ProtoSphericalCollider b)
        {
            var sqrDistance = Vector3.SqrMagnitude(b.transform.position - a.transform.position);
            var radiiSum = a.Size.sqrMagnitude + b.Size.sqrMagnitude;
            return sqrDistance <= radiiSum;
        }

        public static bool DoOverlap(ProtoBoxCollider a, ProtoBoxCollider b)
        {
            // bool does = false;
            // if (a.PosVertex.x >= b.NegVertex.x && a.PosVertex.x <= b.PosVertex.x)
            // {
            //     Debug.Log();
            // }
            // return does;
            // return
            //     (a.PosVertex.x >= b.NegVertex.x && a.PosVertex.x <= b.PosVertex.x)
            //     || (a.PosVertex.y >= b.NegVertex.y && a.PosVertex.y <= b.PosVertex.y)
            //     || (a.PosVertex.z >= b.NegVertex.z && a.PosVertex.z <= b.PosVertex.z) 
            //     ||
            //     (a.NegVertex.x <= b.PosVertex.x && a.NegVertex.x >= b.NegVertex.x) 
            //     || (a.NegVertex.y <= b.PosVertex.y && a.NegVertex.y >= b.NegVertex.y) 
            //     || (a.NegVertex.z <= b.PosVertex.z && a.NegVertex.z >= b.NegVertex.z);

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