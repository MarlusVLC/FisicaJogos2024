using UnityEngine;

namespace _2._RevisaoVetores
{
    public class MeshTransformer3D : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [Space]
        [SerializeField] private float txSpeed;
        [SerializeField] private float tySpeed;
        [SerializeField] private float tzSpeed;
        [Space]
        [SerializeField] private float sxSpeed;
        [SerializeField] private float sySpeed;
        [SerializeField] private float szSpeed;
        [Space]
        [SerializeField] private float rxSpeed;
        [SerializeField] private float rySpeed;
        [SerializeField] private float rzSpeed;

        [Space] [SerializeField] private Vector2 targetPosition;

        private Mesh mesh => meshFilter.mesh;
        
        private void Update()
        {
            Translate3D(txSpeed * Time.deltaTime, tySpeed * Time.deltaTime, tzSpeed * Time.deltaTime);
            Scale3D(sxSpeed, sySpeed, szSpeed);
            // RotateX(rxSpeed * Time.deltaTime);
            // RotateY(rySpeed * Time.deltaTime);
            // RotateZ(rzSpeed * Time.deltaTime);
            Rotate3D(rxSpeed * Time.deltaTime, rySpeed * Time.deltaTime, rzSpeed * Time.deltaTime);
        }

        private void Translate3D(float tx, float ty, float tz)
        {
            var vertices = mesh.vertices;
            float[,] mat =
            {
                { 1, 0, 0, tx }, 
                { 0, 1, 0, ty },
                { 0, 0, 1, tz },
                { 0, 0, 0, 1 }
            };
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)mesh.vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }
        
        private void Scale3D(float sx, float sy, float sz)
        {
            var vertices = mesh.vertices;
            float[,] mat =
            {
                { sx, 0, 0, 0 }, 
                { 0, sy, 0, 0 },
                { 0, 0, sz, 0 },
                { 0, 0, 0, 1 }
            };
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)mesh.vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }
        
        private void RotateX(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians) angle *= Mathf.Deg2Rad;
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            float[,] mat =
            {
                { 1, 0, 0, 0 }, 
                { 0, cos, -sin, 0 },
                { 0,   sin,   cos, 0 },
                {0,    0,   0, 1 }
            };
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)mesh.vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }
        
        private void RotateY(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians) angle *= Mathf.Deg2Rad;
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            float[,] mat =
            {
                { cos, 0, sin, 0 }, 
                { 0, 1, 0, 0 },
                { -sin,   0,   cos, 0 },
                {0,    0,   0, 1 }
            };
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)mesh.vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }

        private void RotateZ(float angle, bool convertToRadians = true)
        {
            var vertices = mesh.vertices;
            if (convertToRadians) angle *= Mathf.Deg2Rad;
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            float[,] mat =
            {
                { cos, -sin, 0, 0 }, 
                { sin, cos, 0, 0 },
                { 0,   0,   1, 0 },
                {0,    0,   0, 1 }
            };
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (VectorN)mesh.vertices[i] * mat;
            }
            mesh.vertices = vertices;
        }

        private void Rotate3D(float angleX, float angleY, float angleZ, bool convertToRadians = true)
        {
            if (convertToRadians)
            {
                angleX *= Mathf.Deg2Rad;
                angleY *= Mathf.Deg2Rad;
                angleZ *= Mathf.Deg2Rad;
            }
            var cos = Mathf.Cos(angleX);
            var sin = Mathf.Sin(angleX);
            float[,] matX =
            {
                { 1, 0, 0, 0 }, 
                { 0, cos, -sin, 0 },
                { 0,   sin,   cos, 0 },
                {0,    0,   0, 1 }
            };
            cos = Mathf.Cos(angleY);
            sin = Mathf.Sin(angleY);
            float[,] matY =
            {
                { cos, 0, sin, 0 }, 
                { 0, 1, 0, 0 },
                { -sin,   0,   cos, 0 },
                {0,    0,   0, 1 }
            };
            cos = Mathf.Cos(angleZ);
            sin = Mathf.Sin(angleZ);
            float[,] matZ =
            {
                { cos, -sin, 0, 0 }, 
                { sin, cos, 0, 0 },
                { 0,   0,   1, 0 },
                {0,    0,   0, 1 }
            };
            var mat = MathH.Multiply(matX, MathH.Multiply(matY, matZ));
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                // vertices[i] = VectorN.MultiplyByHomogeneousMatrix(mesh.vertices[i], mat);
                vertices[i] = (VectorN)mesh.vertices[i] * mat;    

            }
            mesh.vertices = vertices;
        }
        
        //TODO: Generalizar as transformações para N dimensões.
    }
}