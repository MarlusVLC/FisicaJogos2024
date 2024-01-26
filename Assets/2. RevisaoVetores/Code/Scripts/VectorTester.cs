using System;
using UnityEngine;

namespace _2._RevisaoVetores
{
    public class VectorTester : MonoBehaviour
    {
        [SerializeField] private CustomVector a;
        [SerializeField] private CustomVector b;

        [ContextMenu("Sum Into A")]
        public void SumIntoA() => a += b;
        
        [ContextMenu("Decrease A")]
        public void DecreaseA() => a -= b;

        [ContextMenu("Normalize A")] 
        public void NormalizeA() => a = a.Normalize();

        [ContextMenu("Duplicate A")]
        public void DuplicateA() => a *= 2;
        
        [ContextMenu("Half A")]
        public void HalfA() => a /= 2;
        
        [ContextMenu("Show A's Magnitude")]
        public void ShowAMagnitude() => Debug.Log(a.Magnitude());

        [ContextMenu("Sum B's Double Into A")]
        public void SumBDoubleIntoA() => a = a + b * 2;

        [ContextMenu("Negate A")]
        public void NegateA() => a *= -1;

        [ContextMenu("Show Distance Between A and B")]
        public void ShowDistanceBetween() => Debug.Log(CustomVector.Distance(a, b));

        [ContextMenu("Show Dot Product")]
        public void ShowDotProduct()
        {
            Debug.Log($"Unity Vector Dot Product = {Vector3.Dot(a,b)}");
            Debug.Log($"Custom Vector Dot Product = {CustomVector.DotProduct(a,b)}");
        }

        [ContextMenu("Show Angle Between")]
        public void ShowAngleBetween()
        {
            Debug.Log($"Angle Between A and B in radians = {CustomVector.Angle(a,b)}");
            Debug.Log($"Angle Between A and B in degrees = {CustomVector.Angle(a,b, true)}");
        }

        [ContextMenu("Show Component of B onto A")]
        public void ShowComponentOfBontoA()
        {
            Debug.Log($"Component of B onto A = {CustomVector.Component(b, a)}");
        }
    }
}