using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierpinskiCarpet : MonoBehaviour
{
    [SerializeField] private Shader colorShader;

    [SerializeField] private int maxDepth;

    [SerializeField] private float size;

    // [SerializeField] private bool changeColor = false;
    
    
    private List<GameObject> quads = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        DrawCarpet(
           0,
           maxDepth,
           new Vector2(-size,-size),
           new Vector2(size,-size),
           new Vector2(-size,size),
           new Vector2(size,size)
       );
    }

    // Update is called once per frame

    
    void DrawCarpet(int level, int maxLevels, Vector2 dl, Vector2 dr, Vector2 ur, Vector2 ul)
    {
        level++;
        if (level > maxLevels)  
            return;

        if (level % 2 == 0)
        {
            CreateQuad(dl, dr, ur, ul, -level,Color.black);
        }
        else
            CreateQuad(dl, dr, ur, ul, -level,Color.white);
        
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                    continue;
                Vector2 a = new Vector2(ul.x + i * (ur.x - ul.x) / 3, ul.y + j * (dl.y - ul.y) / 3);
                Vector2 b = new Vector2(ul.x + (i+1) * (ur.x - ul.x) / 3, ul.y + j * (dl.y - ul.y) / 3);
                Vector2 c = new Vector2(ul.x + (i) * (ur.x - ul.x) / 3, ul.y + (j+1) * (dl.y - ul.y) / 3);
                Vector2 d = new Vector2(ul.x + (i+1) * (ur.x - ul.x) / 3, ul.y + (j+1) * (dl.y - ul.y) / 3);

                DrawCarpet(level, maxLevels,a, b, c, d);

            }
        }
    }
    
    
    void CreateQuad(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3, int layer, Color color)
    {
        GameObject obj = new GameObject("Quad_" + layer);
        obj.transform.parent = transform;
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();

        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices = new Vector3[]
        {
            new(v0.x,v0.y,0.01f*layer),
            new(v1.x,v1.y,0.01f*layer),
            new(v2.x,v2.y,0.01f*layer),
            new(v3.x,v3.y,0.01f*layer),
        };

        mesh.triangles = new []
        {
            0,2,1,
            2,3,1
        };

        var rend = obj.GetComponent<Renderer>();
        rend.material = new Material(colorShader);
        
        defineColor(mesh,color);

        // AddRandomColorToMesh(mesh);
        quads.Add(obj);
    }

    void defineColor(Mesh mesh, Color color)
    {
        Color[] newColor = new Color[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            newColor[i] = color;
        }

        mesh.colors = newColor;
    }
}
