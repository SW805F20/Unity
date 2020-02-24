using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FieldGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public Vector3 anchor1, anchor2, anchor3, anchor4;

    void Awake(){
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start(){
        MakeMeshData();
        CreateMesh();
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
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    /// <summary>
    /// Generates the mesh based on the vertices and triangles variables.
    /// </summary>
    void CreateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
