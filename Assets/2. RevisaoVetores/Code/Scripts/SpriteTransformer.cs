using UnityEngine;

namespace _2._RevisaoVetores
{
    public class SpriteTransformer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Sprite sprite => spriteRenderer.sprite;

        private void Update()
        {
            Translate(2 * Time.deltaTime,2 * Time.deltaTime);
        }

        private void Translate(float tx, float ty)
        {
            var vertices = sprite.vertices;
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
                vertices[i] = VectorN.MultiplyByHomogeneousMatrix(sprite.vertices[i], mat, false);
            }
            sprite.OverrideGeometry(vertices, sprite.triangles);
        }
    }
}