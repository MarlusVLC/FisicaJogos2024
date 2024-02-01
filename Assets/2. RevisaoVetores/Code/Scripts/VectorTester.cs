using System;
using UnityEngine;

namespace _2._RevisaoVetores
{
    public class VectorTester : MonoBehaviour
    {
        [SerializeField] private VectorN a;
        [SerializeField] private VectorN b;

        private float[,] customMatrix = { { 2, 0}, { 0, 2}};
        // private float[,] customHomogeneousMatrix = { { 1, 0, 2 }, { 0, 1, 2}, {0, 0, 1}};
        private float[,] customHomogeneousMatrix = { { 2, 0, 0 }, { 0, 2, 0}, {0, 0, 1}};

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
        public void ShowDistanceBetween() => Debug.Log(VectorN.Distance(a, b));

        [ContextMenu("Show Dot Product")]
        public void ShowDotProduct()
        {
            Debug.Log($"Unity Vector Dot Product = {Vector3.Dot(a,b)}");
            Debug.Log($"Custom Vector Dot Product = {VectorN.DotProduct(a,b)}");
        }

        [ContextMenu("Show Angle Between")]
        public void ShowAngleBetween()
        {
            Debug.Log($"Angle Between A and B in radians = {VectorN.Angle(a,b)}");
            Debug.Log($"Angle Between A and B in degrees = {VectorN.Angle(a,b, true)}");
        }

        [ContextMenu("Show Component of B onto A")]
        public void ShowComponentOfBontoA()
        {
            Debug.Log($"Component of B onto A = {VectorN.Component(b, a)}");
        }

        [ContextMenu("Show Projection of B onto A")]
        public void ShowProjectionOfBontoA()
        {
            Debug.Log($"Projection of B onto A = {VectorN.Project(b,a)}");
        }

        [ContextMenu("Multiply A by Custom Matrix")]
        public void MultiplyAbyMatrix() => a = a * customMatrix;

        [ContextMenu("Multiply A by Homogeneous Matrix")]
        public void MultiplyAbyHomogeneousMatrix() => a = VectorN.MultiplyByHomogeneousMatrix(a, customHomogeneousMatrix);
    }
}