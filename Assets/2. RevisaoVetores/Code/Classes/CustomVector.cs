using System;
using System.Linq;
using UnityEngine;

namespace _2._RevisaoVetores
{
    [Serializable]
    public class CustomVector
    {
        public float[] vertices;

        public CustomVector(params float[] vertices) => this.vertices = vertices;

        public static CustomVector operator +(CustomVector a, CustomVector b)
        {
            if (a.vertices.Length != b.vertices.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var valueArray = new float[a.vertices.Length];
            for (var i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = a.vertices[i] + b.vertices[i];
            }
            return new CustomVector(valueArray);
        }
    
        public static CustomVector operator *(CustomVector a, float k)
        {
            var valueArray = new float[a.vertices.Length];
            for (var i = 0; i < a.vertices.Length; i++)
            {
                valueArray[i] = a.vertices[i] * k;
            }
            return new CustomVector(valueArray);
        }

        public static CustomVector operator /(CustomVector a, float k) => a * (1 / k);

        public static CustomVector operator -(CustomVector a, CustomVector b) => a + b*-1;

        public float Magnitude() => Mathf.Sqrt(vertices.Sum(element => element * element));

        public CustomVector Normalize() => this / Magnitude();

        public static implicit operator Vector3(CustomVector v)
        {
            if (v.vertices.Length > 3)
            {
                throw new Exception("A game ready vector shouldn't have more than 3 vertices!");
            }
            return new Vector3(v.vertices[0], v.vertices[1], v.vertices[2]);
        }
        
        public static implicit operator CustomVector(Vector3 v)
        {
            return new CustomVector(v.x, v.y, v.z);
        }

        public override string ToString()
        {
            var rep = "(";
            for (var i = 0; i < vertices.Length; i++)
            {
                if (i > 0)
                {
                    rep += " ";
                }
                rep += vertices[i];
                if (i < vertices.Length - 1)
                {
                    rep += ",";
                }
            }
            rep += ")";
            return rep;
        }

        public static float Distance(CustomVector a, CustomVector b)
        {
            if (a.vertices.Length != b.vertices.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.vertices.Length; i++)
            {
                result += (a.vertices[i] - b.vertices[i]) * (a.vertices[i] - b.vertices[i]);
            }
            // var result = a.vertices.Select((t, i) => (t - b.vertices[i]) * (t - b.vertices[i])).Sum();
            return Mathf.Sqrt(result);
        }

        public static float DotProduct(CustomVector a, CustomVector b)
        {
            if (a.vertices.Length != b.vertices.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.vertices.Length; i++)
            {
                result += a.vertices[i] * b.vertices[i];
            }
            return result;
            // return a.vertices.Select((t, i) => t * b.vertices[i]).Sum();
        }

        public static float Angle(CustomVector from, CustomVector to, bool convertToDegrees = false) =>
            Mathf.Acos(DotProduct(from, to) / (from.Magnitude() * to.Magnitude())) *
            (convertToDegrees ? 180 / Mathf.PI : 1);
    }
}

