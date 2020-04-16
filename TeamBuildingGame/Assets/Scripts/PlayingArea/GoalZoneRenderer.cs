using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZoneRenderer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>This method uses the anchor positions to draw the goalzone based on triangles.</summary>
    /// <param name="anchor1">The bottom-left anchor for the goalzone.</param>
    /// <param name="anchor2">The top-left anchor for the goalzone.</param>
    /// <param name="anchor3">The bottom-right anchor for the goalzone.</param>
    /// <param name="anchor4">The top-right anchor for the goalzone.</param>
    public void MakeMeshData(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3, Vector4 anchor4)
    {
        mesh = GetComponent<MeshFilter>().mesh;
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
