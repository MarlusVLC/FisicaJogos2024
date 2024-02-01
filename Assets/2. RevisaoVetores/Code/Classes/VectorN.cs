using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace _2._RevisaoVetores
{
    [Serializable]
    public class VectorN
    {
        public float[] dimensionsValues;

        public VectorN(params float[] dimensionsValues) => this.dimensionsValues = dimensionsValues;

        public VectorN(uint length)
        {
            dimensionsValues = new float[length];
            for (var i = 0; i < length; i++)
            {
                dimensionsValues[i] = 0;
            }
        }

        public int Length => dimensionsValues.Length;

        [System.Runtime.CompilerServices.IndexerName("DimensionValue")]
        public float this[int index]
        {
            get => dimensionsValues[index];
            set => dimensionsValues[index] = value;
        }

        public static VectorN operator +(VectorN a, VectorN b)
        {
            if (a.dimensionsValues.Length != b.dimensionsValues.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var valueArray = new float[a.dimensionsValues.Length];
            for (var i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = a.dimensionsValues[i] + b.dimensionsValues[i];
            }
            return new VectorN(valueArray);
        }
    
        public static VectorN operator *(VectorN a, float k)
        {
            var valueArray = new float[a.dimensionsValues.Length];
            for (var i = 0; i < a.dimensionsValues.Length; i++)
            {
                valueArray[i] = a.dimensionsValues[i] * k;
            }
            return new VectorN(valueArray);
        }

        public static VectorN operator *(VectorN vector, float[,] matrix)
        {
            var result = new VectorN(vector.dimensionsValues);
            Debug.Log(result);
            for (var row = 0; row < matrix.GetLength(0); row++)
            {
                var rowSum = 0f;
                for (var column = 0; column < matrix.GetLength(1); column++)
                {
                    Debug.Log($"Row = {row}");
                    Debug.Log($"Column = {column}");
                    Debug.Log($"Matrix ACCESSED value = {matrix[row, column]}");
                    Debug.Log($"Vector accessed Value = {vector[column]}");
                    rowSum += matrix[row, column] * vector.dimensionsValues[column];
                    Debug.Log($"Row sum = {rowSum}");
                    Debug.Log($"-----------");
                }
                result.dimensionsValues[row] = rowSum;
            }
            return result;
        }

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
            Debug.Log($"Just Created Temp Vector = {result}");
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
            Debug.Log($"Adapted Temp Vector = {result}");
            result *= matrix;
            if (preserveExtraDimensions)
            {
                for (var i = vectorDimensions - 1; i < matrixDimensions; i++)
                {
                    result[i] = vector[i];
                }
            }
            Debug.Log($"Multiplied Temp Vector = {result}");
            return result;
            
        }

        public static VectorN operator /(VectorN a, float k) => a * (1 / k);

        public static VectorN operator -(VectorN a, VectorN b) => a + b*-1;

        public float Magnitude() => Mathf.Sqrt(dimensionsValues.Sum(element => element * element));

        public VectorN Normalize() => this / Magnitude();

        public static implicit operator Vector3(VectorN v)
        {
            if (v.dimensionsValues.Length > 3)
            {
                throw new Exception("A game ready vector shouldn't have more than 3 vertices!");
            }
            return new Vector3(v.dimensionsValues[0], v.dimensionsValues[1], v.dimensionsValues[2]);
        }
        
        public static implicit operator VectorN(Vector3 v)
        {
            return new VectorN(v.x, v.y, v.z);
        }

        public override string ToString()
        {
            var rep = "(";
            for (var i = 0; i < dimensionsValues.Length; i++)
            {
                if (i > 0)
                {
                    rep += " ";
                }
                rep += dimensionsValues[i];
                if (i < dimensionsValues.Length - 1)
                {
                    rep += ",";
                }
            }
            rep += ")";
            return rep;
        }

        public static float Distance(VectorN a, VectorN b)
        {
            if (a.dimensionsValues.Length != b.dimensionsValues.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.dimensionsValues.Length; i++)
            {
                result += (a.dimensionsValues[i] - b.dimensionsValues[i]) * (a.dimensionsValues[i] - b.dimensionsValues[i]);
            }
            // var result = a.vertices.Select((t, i) => (t - b.vertices[i]) * (t - b.vertices[i])).Sum();
            return Mathf.Sqrt(result);
        }

        public static float DotProduct(VectorN a, VectorN b)
        {
            if (a.dimensionsValues.Length != b.dimensionsValues.Length)
                throw new Exception("Vectors should have same amount of vertices");
            var result = 0f;
            for (var i = 0; i < a.dimensionsValues.Length; i++)
            {
                result += a.dimensionsValues[i] * b.dimensionsValues[i];
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

