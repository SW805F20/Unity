﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZoneRenderer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    /// <summary>This method uses the anchor positions to draw the goalzone based on triangles.</summary>
    /// <param name="corners">All corners of the goal zone.</param>
    public void MakeMeshData(Vector3[] corners)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        // Define the four corners of the playing field, going in a clockwise direction.
        vertices = corners;
        // Define how the triangles should be constructed for the mesh.
        // Should go from index 0 to 1 to 2 for first triangle and then
        // 2, 3, 0 for the second to construct a square.
        triangles = new int[] { 0, 1, 2, 2, 3, 0};

        CreateMesh();
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
