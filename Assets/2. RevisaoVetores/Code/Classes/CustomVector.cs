using System;
using System.Linq;
using UnityEngine;

namespace _2._RevisaoVetores
{
    [Serializable]
    public class CustomVector
    {
        public float[] Array;

        public CustomVector(float[] array) => Array = array;

        public static CustomVector operator +(CustomVector a, CustomVector b)
        {
            if (a.Array.Length != b.Array.Length)
                throw new Exception("Vectors should have same length");
            var valueArray = new float[a.Array.Length];
            for (var i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = a.Array[i] + b.Array[i];
            }
            return new CustomVector(valueArray);
        }
    
        public static CustomVector operator *(CustomVector a, float k)
        {
            var valueArray = new float[a.Array.Length];
            for (var i = 0; i < a.Array.Length; i++)
            {
                Debug.Log($"Pre-Operation Value = {a.Array[i]}");
                valueArray[i] = a.Array[i] * k;
                Debug.Log($"Multiplier = {k}");
                Debug.Log($"Multiplied Value = {valueArray[i]}");
            }
            return new CustomVector(valueArray);
        }

        public static CustomVector operator /(CustomVector a, float k) => a * (1 / k);

        public static CustomVector operator -(CustomVector a, CustomVector b) => a + b*-1;

        public float Magnitude() => Mathf.Sqrt(Array.Sum(element => element * element));

        public CustomVector Normalize()
        {
            var valueArray = new float[Array.Length];
            for (var i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = Array[i] / Magnitude();
            }
            return new CustomVector(valueArray);
        }
    }
}

