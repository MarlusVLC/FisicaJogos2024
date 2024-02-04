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
        [Min(1)][SerializeField] private int maxDepth;
    
        private List<TriangleCoordinates> triangleData;
        private List<GameObject> trianglesObjects;

        private void Start()
        {
            DrawSierpinsky();
        }

        public void DrawSierpinsky()
        {
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
            var baseCoordinates = TriangleCoordinates.CreateEquilateralTriangle(sideLength, transform.position);
            SetTriangleCoordinates(1, maxDepth, baseCoordinates, triangleData);
            GenerateTrianglesGameObjects();
        }

        private void SetTriangleCoordinates(int currentLevel,  int maxLevels, TriangleCoordinates targetCoordinates, List<TriangleCoordinates> sierpinskyCoordinates)
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
            var leftTop = left + (top - left) / 2;
            var rightTop = right + (top - right) / 2;
            var baseCenter = left + (right - left) / 2;
            currentLevel++;

            SetTriangleCoordinates(currentLevel,maxLevels, new TriangleCoordinates(right, baseCenter, rightTop),sierpinskyCoordinates);
            SetTriangleCoordinates(currentLevel,maxLevels,new TriangleCoordinates(baseCenter, left, leftTop),sierpinskyCoordinates);
            SetTriangleCoordinates(currentLevel,maxLevels,new TriangleCoordinates(rightTop, leftTop, top),sierpinskyCoordinates);
        }

        private void GenerateTrianglesGameObjects()
        {
            var recycleGameObjects = trianglesObjects.Count > triangleData.Count;
            var iterationTarget = recycleGameObjects ? trianglesObjects.Count : triangleData.Count;
            for (var i = 0; i < triangleData.Count; i++)
            {
                
            }
        }
    }    
}

