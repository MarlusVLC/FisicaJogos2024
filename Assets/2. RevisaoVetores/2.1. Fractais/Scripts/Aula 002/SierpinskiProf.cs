using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierpinskiProf : MonoBehaviour
{
    public Shader colorShader;

    public int maxDepth;

    public bool changeColor = false;

    List<GameObject> triangles = new List<GameObject>();

    // Start is called before the first frame update

    void Start()

    {

        DrawSierpiensky(0,

                        maxDepth,

                        new Vector2(-6,-4),

                        new Vector2(0,4),

                        new Vector2(6,-4));

    }



    // Update is called once per frame

    void Update()

    {

        if(changeColor){

            int rand = Random.Range(0,triangles.Count);

            Mesh mesh = triangles[rand].GetComponent<MeshFilter>().mesh;

            AddRandomColorToMesh(mesh);

        }

        

    }

    void DrawSierpiensky(int level,int maxLevels,Vector2 l,Vector2 t,

                            Vector2 r){

        level++;

        if(level > maxLevels){

            return;

        }

        CreateTriangle(l,t,r,-level);



        Vector2 a = l + (t-l)/2;

        Vector2 b = r + (t-r)/2;

        Vector2 c = l + (r-l)/2;



        DrawSierpiensky(level,maxLevels,a,t,b);

        DrawSierpiensky(level,maxLevels,l,a,c);

        DrawSierpiensky(level,maxLevels,c,b,r);



    }



    void CreateTriangle(Vector2 l,Vector2 t,Vector2 r,int layer){

        GameObject obj = new GameObject("Tri_"+layer);

        obj.transform.parent = transform;

        obj.AddComponent<MeshFilter>();

        obj.AddComponent<MeshRenderer>();



        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;

        mesh.Clear();



        mesh.vertices = new Vector3[]{

            new Vector3(l.x,l.y,0.01f*layer),

            new Vector3(t.x,t.y,0.01f*layer),

            new Vector3(r.x,r.y,0.01f*layer)

        };

        mesh.uv = new Vector2[]{

            new Vector2(0,0),

            new Vector2(0,1),

            new Vector2(1,1)

            };

        mesh.triangles = new int[]{0,1,2};



        Renderer rend = obj.GetComponent<Renderer>();

        rend.material = new Material(colorShader);



        AddRandomColorToMesh(mesh);

        triangles.Add(obj);

    }

    void AddRandomColorToMesh(Mesh mesh){

        Color randColor = new Color(Random.Range(0.0f,1.0f),

                                    Random.Range(0.0f,1.0f),

                                    Random.Range(0.0f,1.0f),

                                    1.0f);



        mesh.colors = new Color[]{

            randColor,randColor,randColor

        };

    }
}
