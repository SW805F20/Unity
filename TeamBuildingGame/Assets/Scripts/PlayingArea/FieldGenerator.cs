﻿using UnityEngine;

public delegate void FieldCreatedEvent(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3, Vector3 anchor4);

[RequireComponent(typeof(MeshFilter))]
public class FieldGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public Vector3 anchor1, anchor2, anchor3, anchor4;
    public bool fieldCreated = false;
    public event FieldCreatedEvent OnFieldCreated;
    public GameObject goalZoneHandler;

    void Awake(){
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void CreatePlayingField(){
        MakeMeshData();
        CreateMesh();
        DefineUVs();
        FieldCreatedEventHandler();
    }
    private void FieldCreatedEventHandler()
    {
        OnFieldCreated?.Invoke(anchor1, anchor2, anchor3, anchor4);
    }

    private void Update()
    {
        if (fieldCreated)
        {
            goalZoneHandler.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the mesh data to prepare for making a new mesh.
    /// </summary>
    void MakeMeshData(){
        // Define the four corners of the playing field, going in a clockwise direction.
        vertices = new Vector3[]{ anchor1, anchor2, anchor3, anchor4 };
        // Define how the triangles should be constructed for the mesh.
        // Should go from index 0 to 1 to 2 for first triangle and then
        // 2, 1, 3 for the second to construct a square.
        triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    }

    /// <summary>
    /// Generates the mesh based on the vertices and triangles variables.
    /// </summary>
    void CreateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        fieldCreated = true;
    }

    /// <summary>
    /// UVs are defined as the base texture coordinates for a mesh. If they are not explicitly defined, the mesh cannot render a picture.
    /// Instead, it will just render a single color. So the coordinates are defined as the anchors for the playing field, as that is how big the mesh is.
    /// </summary>
    void DefineUVs()
    {
        // This is necessary to make a mesh display an actual texture, otherwise it just displays a single color.
        Vector2 corner1 = new Vector2(anchor1.x, anchor1.y);
        Vector2 corner2 = new Vector2(anchor2.x, anchor2.y);
        Vector2 corner3 = new Vector2(anchor3.x, anchor3.y);
        Vector2 corner4 = new Vector2(anchor4.x, anchor4.y);
        Vector2[] uvs = new Vector2[4] { corner1, corner2, corner3, corner4};

        mesh.uv = uvs;
    }
}
