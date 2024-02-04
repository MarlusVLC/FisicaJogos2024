using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _2._RevisaoVetores
{
    public class TriangleMaker : MonoBehaviour
    {
        public Shader colorShader;

        public GameObject CreateTriangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            var obj = new GameObject("Triangle");
            var mesh = obj.AddComponent<MeshFilter>().mesh;
            var renderer = obj.AddComponent<MeshRenderer>();
            mesh.Clear();
            mesh.vertices = new Vector3[]
            {
                new(v0.x,v0.y),
                new(v1.x,v1.y),
                new(v2.x,v2.y),
            };
            renderer.material = new Material(colorShader);
            mesh.triangles = new[] {0,1,2};
            mesh.colors = new Color[mesh.vertexCount];
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                mesh.colors[i] = Color.black;
            }
            return obj;
        }
        
        public GameObject CreateTriangle(TriangleCoordinates coordinates) =>
            CreateTriangle(coordinates.right, coordinates.left, coordinates.top);

        public void RecycleTriangle(GameObject triangleObject, Vector2 v0, Vector2 v1, Vector2 v2)
        {
            var mesh = triangleObject.GetComponent<MeshFilter>().mesh;
            if (mesh.vertexCount == 3)
            {
                throw new Exception("The object must be a triangle");
            }
            mesh.Clear();
            mesh.vertices = new Vector3[]
            {
                new(v0.x,v0.y),
                new(v1.x,v1.y),
                new(v2.x,v2.y),
            };
        }
         
        public GameObject CreateTrianglesInMesh(TriangleCoordinates[] coordinates)
        {
            var obj = new GameObject("Triangle");
            var mesh = obj.AddComponent<MeshFilter>().mesh;
            var renderer = obj.AddComponent<MeshRenderer>();
            mesh.Clear();
            var pointQuantity = coordinates.Count() * 3;
            var vertices = new Vector3[pointQuantity];
            var triangleIndexes = new int[pointQuantity];
            var i = 0;
            var j = 0;
            while (i < pointQuantity)
            {
                vertices[i] = coordinates[j].right;
                triangleIndexes[i] = i++;
                vertices[i] = coordinates[j].left;
                triangleIndexes[i] = i++;
                vertices[i] = coordinates[j].top;
                triangleIndexes[i] = i++;
                j++;
            }
            mesh.vertices = vertices;
            renderer.material = new Material(colorShader);
            mesh.triangles = triangleIndexes;
            mesh.colors = new Color[mesh.vertexCount];
            for (i = 0; i < mesh.vertexCount; i++)
            {
                mesh.colors[i] = Color.black;
            }
            return obj;
        }

        public GameObject CreateEquilateralTriangle(float sideLength)
        {
            var top = new Vector2(0, sideLength / Mathf.Sqrt(3));
            var baseEdges = sideLength / (2 * Mathf.Sqrt(3));
            var left = new Vector2(-sideLength / 2, -baseEdges);
            var right = new Vector2(sideLength / 2, -baseEdges);
            return CreateTriangle(right, left, top);
        }


    }
}