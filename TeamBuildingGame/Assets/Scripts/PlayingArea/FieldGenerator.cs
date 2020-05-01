using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FieldGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public bool fieldCreated = false;

    public GameObject goalZoneHandler;
    GameStateHandler gameStateHandler;

    void Awake(){
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void CreatePlayingField(){
        MakeMeshData();
        CreateMesh();
        DefineUVs();
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
        vertices = new Vector3[]{ gameStateHandler.anchor1, gameStateHandler.anchor2, gameStateHandler.anchor3, gameStateHandler.anchor4 };
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
        Vector2 corner1 = new Vector2(gameStateHandler.anchor1.x, gameStateHandler.anchor1.y);
        Vector2 corner2 = new Vector2(gameStateHandler.anchor2.x, gameStateHandler.anchor2.y);
        Vector2 corner3 = new Vector2(gameStateHandler.anchor3.x, gameStateHandler.anchor3.y);
        Vector2 corner4 = new Vector2(gameStateHandler.anchor4.x, gameStateHandler.anchor4.y);
        Vector2[] uvs = new Vector2[4] { corner1, corner2, corner3, corner4};

        mesh.uv = uvs;
    }
}
