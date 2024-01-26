using System;
using UnityEngine;

namespace _2._RevisaoVetores
{
    public class MeshTransformer : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [Space] [SerializeField] private float tx, ty;

        private Mesh mesh => meshFilter.mesh;
        
        private void Translate2D(float tx, float ty)
        {
            var vertices = mesh.vertices;
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                vertices[i] = new Vector3(vertices[i].x + tx, vertices[i].y + ty);
            }

            mesh.vertices = vertices;
        }

        private void Update()
        {
            Translate2D(tx * Time.deltaTime, ty * Time.deltaTime);
        }
    }
}