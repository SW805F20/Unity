using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FieldGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public Vector3 anchor1, anchor2, anchor3, anchor4;
    public Camera mainCamera;
    public GameObject cameraRig;
    private Transform cameraTransform;
    GvrControllerInputDevice ControllerInputDevice;
    public Quaternion orientation;
    public GvrControllerInput gvrControllerMain;

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

    void Update()
    {
        /*
        float x = mainCamera.transform.position.x;
        var y = mainCamera.transform.position.y;
        var z = mainCamera.transform.position.z * 10;
        Quaternion orientation = Gvr.GvrControllerInput.Orientation;
        orien = GvrControllerInputDevice.Orientation;
        Vector3 forward = GvrVRHelpers.GetHeadForward();
        forward.z = forward.z + 10;
        transform.position = forward;*/
        /* Vector3 position = mainCamera.transform.localPosition;

         position.z = position.z = -10;
         transform.localPosition = position;
         transform.localRotation = mainCamera.transform.localRotation;*/

        /*Vector3 resultingPosition = mainCamera.transform.position + mainCamera.transform.forward * 10;
        transform.position = new Vector3(resultingPosition.x, resultingPosition.y, resultingPosition.z);
        transform.rotation = mainCamera.transform.localRotation;*/
        Debug.Log(mainCamera.transform.forward);


        Vector3 resultingPosition = mainCamera.transform.position + mainCamera.transform.forward;
        transform.position = new Vector3(resultingPosition.x, resultingPosition.y, resultingPosition.z);
        transform.rotation = mainCamera.transform.localRotation;


    } 
}
