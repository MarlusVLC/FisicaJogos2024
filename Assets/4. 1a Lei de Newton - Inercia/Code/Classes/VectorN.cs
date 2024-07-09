using System;
using System.Linq;
using UnityEngine;

namespace _4.Inercia
{
    [Serializable]
    public struct VectorN
    {
        public float[] components;

        public VectorN(params float[] components)
        {
            if (components == null || components.Length == 0)
            {
                throw new AggregateException("At least one element is required");
            }
            
            this.components = new float[components.Length];
            for (var i = 0; i < components.Length; i++)
            {
                this.components[i] = components[i];
            }
        }

        public VectorN(uint length)
        {
            components = new float[length];
            for (var i = 0; i < length; i++)
            {
                components[i] = 0;
            }
        }

        public int Length => components.Length;

        [System.Runtime.CompilerServices.IndexerName("Dimension")]
        public float this[int index]
        {
            get => components[index];
            set => components[index] = value;
        }

        public static VectorN operator +(VectorN a, VectorN b)
        {
            if (a.components.Length != b.components.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var valueArray = new float[a.components.Length];
            for (var i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = a.components[i] + b.components[i];
            }
            return new VectorN(valueArray);
        }
        
        //Although algebraically correct, this is bad way to do this, as it generates two new vectors. 
        public static VectorN operator -(VectorN a, VectorN b) => a + b*-1;

        public static VectorN operator *(VectorN a, float k)
        {
            var valueArray = new float[a.components.Length];
            for (var i = 0; i < a.components.Length; i++)
            {
                valueArray[i] = a.components[i] * k;
            }
            return new VectorN(valueArray);
        }

        public static VectorN operator *(VectorN vector, float[,] matrix)
        {
            Vector3 result = new VectorN(vector.components);
            for (int r = 0; r < matrix.GetLength(0); r++) 
            { 
                float s = 0;
                for (int z = 0; z < matrix.GetLength(1); z++)
                    s += matrix[r, z] * (z >= vector.Length ? 1 : vector[z]);
                if (r >= vector.Length) break;
                result[r] = s;
            }
            return result;
        }
        
        //Although algebraically correct, this is bad way to do this, as it generates two new vectors. 
        public static VectorN operator /(VectorN a, float k) => a * (1 / k);

        public static VectorN MultiplyByHomogeneousMatrix(VectorN vector, float[,] matrix, bool preserveExtraDimensions = true)
        {
            var matrixDimensions = matrix.GetLength(0);
            if (matrixDimensions != matrix.GetLength(1))
            {
                throw new Exception("Entry matrix should be square, with the same amount of columns and rows");
            }
            var vectorDimensions = vector.Length;
            if (matrixDimensions < vectorDimensions)
            {
                throw new Exception("A homogeneous matrix should be at least the same dimension as the vector!");
            }
            var result = new VectorN((uint)matrixDimensions);
            for (var i = 0; i < matrixDimensions; i++)
            {
                if (i == matrixDimensions - 1)
                {
                    result[i] = 1;
                    continue;
                }
                if (i < vectorDimensions)
                {
                    result[i] = vector[i];
                }
            }
            result *= matrix;
            if (preserveExtraDimensions)
            {
                for (var i = vectorDimensions - 1; i < matrixDimensions; i++)
                {
                    result[i] = vector[i];
                }
            }
            else
            {
                var values = new float[vectorDimensions];
                for (int i = 0; i < vectorDimensions; i++)
                {
                    values[i] = result[i];
                }

                result = new VectorN(values);
            }
            

            
            return result;
            
        }

        public float Magnitude() => Mathf.Sqrt(components.Sum(element => element * element));

        public VectorN Normalize() => this / Magnitude();

        public static implicit operator Vector3(VectorN v)
        {
            if (v.components.Length > 3)
            {
                throw new Exception("A game ready vector shouldn't have more than 3 vertices!");
            }
            return new Vector3(v.components[0], v.components[1], v.components[2]);
        }
        
        public static implicit operator VectorN(Vector3 v)
        {
            return new VectorN(v.x, v.y, v.z);
        }

        public override string ToString()
        {
            var rep = "(";
            for (var i = 0; i < components.Length; i++)
            {
                if (i > 0)
                {
                    rep += " ";
                }
                rep += components[i];
                if (i < components.Length - 1)
                {
                    rep += ",";
                }
            }
            rep += ")";
            return rep;
        }

        public static float Distance(VectorN a, VectorN b)
        {
            if (a.components.Length != b.components.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.components.Length; i++)
            {
                result += (a.components[i] - b.components[i]) * (a.components[i] - b.components[i]);
            }
            // var result = a.vertices.Select((t, i) => (t - b.vertices[i]) * (t - b.vertices[i])).Sum();
            return Mathf.Sqrt(result);
        }

        public static float DotProduct(VectorN a, VectorN b)
        {
            if (a.components.Length != b.components.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.components.Length; i++)
            {
                result += a.components[i] * b.components[i];
            }
            return result;
            // return a.vertices.Select((t, i) => t * b.vertices[i]).Sum();
        }

        public static float Angle(VectorN from, VectorN to, bool convertToDegrees = false) =>
            Mathf.Acos(DotProduct(from, to) / (from.Magnitude() * to.Magnitude())) *
            (convertToDegrees ? 180 / Mathf.PI : 1);

        public static float Component(VectorN from, VectorN onto)
        {
            return from.Magnitude() * Mathf.Cos(Angle(from, onto));
        }
        
        public static VectorN Project(VectorN from, VectorN onto)
        {
            return onto.Normalize() * Component(from, onto);
        }
    }
}

