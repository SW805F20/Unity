using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZoneRenderer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeMeshData(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3, Vector4 anchor4)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Debug.Log(anchor1);
        Debug.Log(anchor2);
        Debug.Log(anchor3);
        Debug.Log(anchor4);

        // Define the four corners of the playing field, going in a clockwise direction.
        vertices = new Vector3[] { anchor1, anchor2, anchor3, anchor4 };
        // Define how the triangles should be constructed for the mesh.
        // Should go from index 0 to 1 to 2 for first triangle and then
        // 2, 1, 3 for the second to construct a square.
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };

        CreateMesh();

    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
