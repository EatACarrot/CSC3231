using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Mesh colliderMesh;


    Texture2D heightMap;
    [SerializeField]
    float Height = 10f;

    MeshRenderer r;

    //displaying the vertecies for manual debugging
    [SerializeField]
    Vector3[] vertices;
    [SerializeField]
    int[] triangles;
    [SerializeField]
    Vector2[] uv;
    [SerializeField]
    Transform player;

    [SerializeField, Range(2,250)]
    int xSize = 20;

    [SerializeField, Range(2, 250)]
    int zSize = 20;
    public int width = 256;
    public int height = 256;
    public float scale = 5f;

    int resolution;
    float offsetX ;
    float offsetY ;

    void Start()
    {
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);
        heightMap = CreateNoise.GenNoiseTexture(width, height, scale, offsetX, offsetY);
        resolution = (xSize + 1) * (zSize + 1);

        mesh = new Mesh();
        mesh.name = "render";
        colliderMesh = new Mesh();
        colliderMesh.name = "collision";



        CreateShape();
        UpdateMesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = colliderMesh;

    }


    void CreateShape()
    {

        //creating vertices and uvs
        vertices = new Vector3[resolution];
        uv = new Vector2[resolution];
        float uGrowth = 1.0f / (float)(xSize + 1);
        float vGrowth = 1.0f / (float)(zSize + 1);


        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float u = (float)x * uGrowth;
                float v = (float)z * vGrowth;
                uv[i] = new Vector2(u, v);
                //mapping the value of the color to the height of the terrain
                float y = heightMap.GetPixelBilinear(u, v).grayscale * Height;
                vertices[i] = new Vector3(x, y * y - 1 , z);
                
                i++;
            }
        }
        
        //creating the triangles buy making a index array
        triangles = new int[(xSize * zSize * 6 )];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }


    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        colliderMesh.Clear();
        colliderMesh.vertices = vertices;
        colliderMesh.triangles = triangles;

        
    }


}
