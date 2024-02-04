using UnityEngine;

namespace _2._RevisaoVetores
{
    public class TriangleCoordinates
    {
        public Vector2 right, left, top;

        public TriangleCoordinates(Vector2 right, Vector2 left, Vector2 top)
        {
            this.right = right;
            this.left = left;
            this.top = top;
        }
        
        public static TriangleCoordinates CreateEquilateralTriangle(float sideLength, Vector2 centerPoint)
        {
            var top = new Vector2(centerPoint.x, centerPoint.y+sideLength / Mathf.Sqrt(3));
            var baseEdges = sideLength / (2 * Mathf.Sqrt(3));
            var left = new Vector2(centerPoint.x-sideLength / 2, centerPoint.y-baseEdges);
            var right = new Vector2(centerPoint.x + sideLength / 2, centerPoint.y-baseEdges);
            return new TriangleCoordinates(right, left, top);
        }
    }
}