using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        CreateMesh();   
    }

    void MakeMeshData(){
        // Define the four corners of the playing field, going in a clockwise direction.
        vertices = new Vector3[]{ p1, p2, p3, p4 };
        // Define how the triangles should be constructed for the mesh
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void CreateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
