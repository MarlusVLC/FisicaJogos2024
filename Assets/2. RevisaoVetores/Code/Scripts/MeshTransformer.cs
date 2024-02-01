using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;

namespace _2._RevisaoVetores
{
    public class MeshTransformer : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [Space]
        [SerializeField] private float txSpeed;
        [SerializeField] private float tySpeed;
        [Space]
        [SerializeField] private float sxSpeed;
        [SerializeField] private float sySpeed;
        [Space] 
        [SerializeField] private float rotationRate;

        private Mesh mesh => meshFilter.mesh;
        
        private void Update()
        {
            // Translate2D(txSpeed * Time.deltaTime, tySpeed * Time.deltaTime);
            // Translate2DMatHM(txSpeed * Time.deltaTime, tySpeed * Time.deltaTime);
            // Scale2D(sxSpeed, sySpeed);
            // Scale2DMat(sxSpeed, sySpeed);
            // Scale2DMatHM(sxSpeed, sySpeed);
            // Rotate2D(rotationRate * Time.deltaTime);
            Rotate2DMat(rotationRate * Time.deltaTime);
            // Rotate2DMatHM(rotationRate * Time.deltaTime);
        }
        
        private void Translate2D(float tx, float ty)
        {
            var vertices = mesh.vertices;
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                vertices[i] = new Vector3(vertices[i].x + tx, vertices[i].y + ty);
            }
            mesh.vertices = vertices;
        }

        private void Translate2DMatHM(float tx, float ty)
        {
            var vertices = mesh.vertices;
            var mat = new float[3, 3];
            mat[0, 0] = 1;
            mat[0, 1] = 0;
            mat[0, 2] = tx;
            mat[1, 0] = 0;
            mat[1, 1] = 1;
            mat[1, 2] = ty;
            mat[2, 0] = 0;
            mat[2, 1] = 0;
            mat[2, 2] = 1;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = VectorN.MultiplyByHomogeneousMatrix(mesh.vertices[i], mat);
            }
            mesh.vertices = vertices;
        }
        
        private void Scale2D(float sx, float sy)
        {
            var vertices = mesh.vertices;
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                vertices[i] = new Vector3(vertices[i].x * sx, vertices[i].y * sy);
            }
            mesh.vertices = vertices;
        }

        private void Scale2DMat(float sx, float sy)
        {
            var vertices = mesh.vertices;
            var mat = new float[2, 2];
            mat[0, 0] = sx;
            mat[0, 1] = 0;
            mat[1, 0] = 0;
            mat[1, 1] = sy;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }
        
        private void Scale2DMatHM(float sx, float sy)
        {
            var vertices = mesh.vertices;
            var mat = new float[3, 3];
            mat[0, 0] = sx;
            mat[0, 1] = 0;
            mat[0, 2] = 0;
            mat[1, 0] = 0;
            mat[1, 1] = sy;
            mat[1, 2] = 0;
            mat[2, 0] = 0;
            mat[2, 1] = 0;
            mat[2, 2] = 1;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = VectorN.MultiplyByHomogeneousMatrix(mesh.vertices[i], mat);
            }
            mesh.vertices = vertices;
        }

        private void Rotate2D(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians)
            {
                angle = angle * Mathf.PI / 180f;
            }
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(
                    vertices[i].x * Mathf.Cos(angle) - vertices[i].y * Mathf.Sin(angle),
                    vertices[i].x * Mathf.Sin(angle) + vertices[i].y * Mathf.Cos(angle));
            }
            mesh.vertices = vertices;
        }

        private void Rotate2DMat(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians)
            {
                angle = angle * Mathf.PI / 180f;
            }

            var mat = new float[2,2];
            mat[0, 0] = Mathf.Cos(angle);
            mat[0, 1] = -Mathf.Sin(angle);
            mat[1, 0] = Mathf.Sin(angle);
            mat[1, 1] = Mathf.Cos(angle);
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }
        
        private void Rotate2DMatHM(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians) 
            {
                angle = angle * Mathf.PI / 180f;
            }

            var mat = new float[3,3];
            mat[0, 0] = Mathf.Cos(angle);
            mat[0, 1] = -Mathf.Sin(angle);
            mat[0, 2] = 0;
            mat[1, 0] = Mathf.Sin(angle);
            mat[1, 1] = Mathf.Cos(angle);
            mat[1, 2] = 0;
            mat[2, 0] = 0;
            mat[2, 1] = 0;
            mat[2, 2] = 1;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = VectorN.MultiplyByHomogeneousMatrix(mesh.vertices[i], mat);
            }
            mesh.vertices = vertices;
        }
    }
}