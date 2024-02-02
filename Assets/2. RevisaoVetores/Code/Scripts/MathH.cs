using UnityEngine;

namespace _2._RevisaoVetores
{
    public static class MathH
    {
        public static Vector3 Multiply(this Vector3 point, float[,] matrix)
        {
            Vector3 result = new Vector3();
            for (int r = 0; r < matrix.GetLength(0); r++) 
            { 
                float s = 0;
                for (int z = 0; z < matrix.GetLength(1); z++)
                    s += matrix[r, z] * (z == 3 ? 1 : point[z]);
                if (r > 2) break;
                result[r] = s;
            }
            return result;
        }

        public static float[,] Multiply(float[,] matrix1, float[,] matrix2)
        {
            var rows = matrix1.GetLength(0);
            var columns = matrix2.GetLength(1);
            var result = new float[rows, columns];
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var sum = 0f;
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        sum += matrix1[row, k] * matrix2[k, column];
                        // Debug.Log($"Matrix 1 [{row+1}, {k+1}] = {matrix1[row, k]} | Matrix 2 [{k+1}, {column+1}] = {matrix2[k, column]}");
                    }
                    // Debug.Log($"Result[{row+1},{column+1}] = {sum}");
                    result[row, column] = sum;
                }
            }
            return result;
        }
    }
}