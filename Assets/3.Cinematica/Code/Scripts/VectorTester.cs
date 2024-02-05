using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3.Cinematica
{
    public class VectorTester : MonoBehaviour
    {
        [SerializeField] private VectorN a;
        [SerializeField] private VectorN b;
        [SerializeField] private float rotationAngle;
        [SerializeField] private int multiplicationIterations;

        private float[,] scaleMatrix2D = { { 2, 0}, { 0, 2}};
        // private float[,] customHomogeneousMatrix = { { 1, 0, 2 }, { 0, 1, 2}, {0, 0, 1}};
        private float[,] translationHomogeneousMatrix2D = { { 1, 0, 2 }, { 0, 1, 2}, {0, 0, 1}};
        private float[,] scaleHomogeneousMatrix2D = { { 2, 0, 0 }, { 0, 1, 0}, {0, 0, 1}};
        private float[,] translationHomogeneousMatrix3D = { { 1, 0, 0, 3 }, { 0, 1, 0, 1}, {0, 0, 1, 10}, {0, 0, 0, 1}};
        // private float[,] translationHomogeneousMatrix3D = { { 1, 0, 0, 3 }, { 0, 1, 0, 1}, {0, 0, 1, 10}, {0, 0, 0, 1}};


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

        [ContextMenu("Multiply A by Scale Matrix 2D")]
        public void MultiplyAbyScaleMatrix2D()
        {
            for (var i = 0; i < multiplicationIterations; i++)
            {
                a = a * scaleMatrix2D;
            }
        } 

        [ContextMenu("Multiply A by Rotation Matrix 2D")]
        public void MultiplyAbyRotationMatrix2D()
        {
            var alpha = rotationAngle * Mathf.Deg2Rad;
            for (var i = 0; i < multiplicationIterations; i++)
            {
                float[,] rotationMatrix2D =
                    { { Mathf.Cos(alpha), -Mathf.Sin(alpha) }, { Mathf.Sin(alpha), Mathf.Cos(alpha) } };
                a = a * rotationMatrix2D;    
            }
        }

        [ContextMenu("Multiply A by 2D Translations Homogeneous Matrix")]
        public void MultiplyAbyTranslationHomogeneousMatrix2D()
        {
            a = VectorN.MultiplyByHomogeneousMatrix(a, translationHomogeneousMatrix2D);
            // a = ((Vector3)a).Multiply(translationHomogeneousMatrix2D);
        }
        
        [ContextMenu("Multiply A by 2D Scale Homogeneous Matrix")]
        public void MultiplyAbyScaleHomogeneousMatrix2D()
        {
            // a = VectorN.MultiplyByHomogeneousMatrix(a, scaleHomogeneousMatrix2D);
            a = ((Vector3)a).Multiply(scaleHomogeneousMatrix2D);
        }
        
        [ContextMenu("Multiply A by 2D Homogeneous Rotation Matrix")]
        public void MultiplyAby2DHomogeneousRotationMatrix()
        {
            var alpha = rotationAngle * Mathf.Deg2Rad;
            var cos = Mathf.Cos(alpha);
            var sin = Mathf.Sin(alpha);
            for (var i = 0; i < multiplicationIterations; i++)
            {
                float[,] rotationMatrix2D =
                {
                    { cos, -sin, 0 }, 
                    { sin, cos, 0 },
                    {0, 0, 1}
                };
                a = a * rotationMatrix2D;    
            }
        }

        [ContextMenu("Multiply Matrix By Matrix")]
        public void ShowMatrixMultiplication()
        {
            var alpha = rotationAngle * Mathf.Deg2Rad;
            var cos = Mathf.Cos(alpha);
            var sin = Mathf.Sin(alpha);
            float[,] rotationMatrix2D =
            {
                { cos, -sin, 0 }, 
                { sin, cos, 0 },
                {0, 0, 1}
            };
            // var a = MathH.Multiply(scaleHomogeneousMatrix2D, rotationMatrix2D);
            var a = MathH.Multiply(rotationMatrix2D, scaleHomogeneousMatrix2D);
        }
        
        [ContextMenu("Multiply A by 3D Translations Homogeneous Matrix")]
        public void MultiplyAbyTranslationHomogeneousMatrix3D()
        {
            a *= translationHomogeneousMatrix3D;
            // a = (Vector3)VectorN.MultiplyByHomogeneousMatrix(a, translationHomogeneousMatrix3D, false);
            // a = ((Vector3)a).Multiply(translationHomogeneousMatrix3D);

        }
    }
}