using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 newPosition;
    private readonly int distanceFromCamera = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // To follow the camera a new position for the transform based on the camera must be found.
        // This is done by taking the position of the camera currently, and also the direction in which the camera is looking with forward.
        // Distance from camera pushes the playingfield further away from the origin of the camera.
        // The rotation is also updated to ensure the view is straight on.
        newPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        transform.position = newPosition;
        transform.rotation = mainCamera.transform.localRotation;
    }
}
