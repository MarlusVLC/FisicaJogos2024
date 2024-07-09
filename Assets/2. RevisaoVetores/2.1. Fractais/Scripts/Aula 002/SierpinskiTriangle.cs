using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace _2._RevisaoVetores
{
    public class SierpinskiTriangle : MonoBehaviour
    {
        [SerializeField] private TriangleMaker triangleMaker;
        [SerializeField] private float sideLength;
    
        private List<TriangleCoordinates> triangleData;
        private List<GameObject> trianglesObjects;


        public void DrawSierpinsky(int maxDepth)
        {
            if (maxDepth < 1)
            {
                DeactivateAllTriangles();
                return;
            }
            var triangleQuantity = (int)Mathf.Pow(3, maxDepth - 1);
            if (trianglesObjects == null)
            {
                trianglesObjects = new List<GameObject>(triangleQuantity);
            }
            else if (triangleQuantity > trianglesObjects.Capacity)
            {
                trianglesObjects.Capacity = triangleQuantity;
            }
            triangleData = new List<TriangleCoordinates>(triangleQuantity);
            var baseCoordinates = TriangleCoordinates.CreateEquilateralTriangle(sideLength, Vector2.zero);
            SetTriangleCoordinates(1, maxDepth, baseCoordinates);
            GenerateTrianglesGameObjects();
        }

        private void SetTriangleCoordinates(int currentLevel,  int maxLevels, TriangleCoordinates targetCoordinates)
        {
            if (currentLevel < 1)
            {
                throw new Exception("There must be at least one level for the triangle to be created!");
            }
            if (currentLevel == maxLevels)
            {
                triangleData.Add(targetCoordinates);
                return;
            }

            var right = targetCoordinates.right;
            var left = targetCoordinates.left;
            var top = targetCoordinates.top;
            var leftTop = (top + left) / 2;
            var rightTop = (top + right) / 2;
            var baseCenter = (right + left) / 2;
            currentLevel++;

            SetTriangleCoordinates(currentLevel,maxLevels, new TriangleCoordinates(right, baseCenter, rightTop));
            SetTriangleCoordinates(currentLevel,maxLevels,new TriangleCoordinates(baseCenter, left, leftTop));
            SetTriangleCoordinates(currentLevel,maxLevels,new TriangleCoordinates(rightTop, leftTop, top));
        }

        private void GenerateTrianglesGameObjects()
        {
            int i;
            for (i = 0; i < triangleData.Count; i++)
            {
                GameObject triangle;
                if (i < trianglesObjects.Count)
                {
                    triangle = trianglesObjects[i];
                    TriangleMaker.RecycleTriangle(trianglesObjects[i], triangleData[i]);
                    trianglesObjects[i].SetActive(true);
                    continue;
                }

                triangle = triangleMaker.CreateTriangle(triangleData[i]);
                trianglesObjects.Add(triangle);
                // triangle.transform.SetParent(transform);
            }

            for (i = triangleData.Count; i < trianglesObjects.Count; i++)
            {
                trianglesObjects[i].SetActive(false);
            }
        }

        private void DeactivateAllTriangles()
        {
            trianglesObjects.ForEach(g => g.SetActive(false));
        }
    }    
}

