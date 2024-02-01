namespace _2._RevisaoVetores
{
    using UnityEngine;

    public class RotationExample : MonoBehaviour
    {
        void Start()
        {
            // Example array of Vector3 (considering only x and y components for 2D rotation)
            Vector3[] vectorsToRotate = {
                new(1.0f, 1.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),
                new(0.0f, -1.0f, 0.0f)
            };

            // Example 2D rotation matrix (45 degrees around the z-axis)
            MatrixNxN rotationMatrix = CreateRotationMatrix(45.0f);

            // Rotate the array of Vector3 using the 2D rotation matrix
            Vector3[] rotatedVectors = RotateVectors(vectorsToRotate, rotationMatrix);

            // Display the results
            Debug.Log("Original Vectors:");
            PrintVectors(vectorsToRotate);

            Debug.Log("Rotated Vectors:");
            PrintVectors(rotatedVectors);
        }

        MatrixNxN CreateRotationMatrix(float angle)
        {
            // Convert the angle to radians
            float radians = Mathf.Deg2Rad * angle;

            // Calculate sine and cosine of the angle
            float cosAngle = Mathf.Cos(radians);
            float sinAngle = Mathf.Sin(radians);

            // Create the 2D rotation matrix
            float[,] elements = new float[,]
            {
                { cosAngle, -sinAngle },
                { sinAngle, cosAngle }
            };

            return new MatrixNxN(elements);
        }

        Vector3[] RotateVectors(Vector3[] vectors, MatrixNxN rotationMatrix)
        {
            Vector3[] rotatedVectors = new Vector3[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                // Ignoring the z-component during rotation (considering 2D rotation)
                rotatedVectors[i] =
                    MultiplyVectorByMatrix(new Vector3(vectors[i].x, vectors[i].y, 0.0f), rotationMatrix);
            }

            return rotatedVectors;
        }

        void PrintVectors(Vector3[] vectors)
        {
            foreach (Vector3 vector in vectors)
            {
                Debug.Log(vector);
            }
        }

        Vector3 MultiplyVectorByMatrix(Vector3 vector, MatrixNxN matrix)
        {
            // Create a new Vector3 to store the result
            Vector3 result = new Vector3();

            // Perform the multiplication
            for (int i = 0; i < matrix.Size; i++)
            {
                result[i] = 0.0f;
                for (int j = 0; j < matrix.Size; j++)
                {
                    result[i] += vector[j] * matrix[i, j];
                }
            }

            return result;
        }

        // Custom nxn matrix class
        public struct MatrixNxN
        {
            private float[,] values;

            public int Size { get; private set; }

            public float this[int row, int col]
            {
                get { return values[row, col]; }
                set { values[row, col] = value; }
            }

            public MatrixNxN(float[,] elements)
            {
                Size = elements.GetLength(0);
                values = new float[Size, Size];

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        values[i, j] = elements[i, j];
                    }
                }
            }
        }
    }
}