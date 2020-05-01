using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 newPosition;

    [SerializeField]
    private float distanceFromCamera = 10;
    private GameStateHandler gameStateHandler;
    private float fieldHeight;
    private float fieldWidth;
    private bool cameraCentered = false;
    private GameObject cameraRig;
    private GameObject playingFieldTransform;


    private void Awake()
    {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        cameraRig = GameObject.Find("Camera Rig");
    }
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = true;
        mainCamera.orthographic = false;
        playingFieldTransform = new GameObject("PlayingFieldTransform");
        playingFieldTransform.transform.SetParent(mainCamera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStateHandler.fieldGeneratorScript.fieldCreated)
        {
            if (!cameraCentered)
            {
                fieldHeight = gameStateHandler.anchor2.y - gameStateHandler.anchor1.y;
                fieldWidth = gameStateHandler.anchor3.x - gameStateHandler.anchor2.x;
                
                Vector3 playingFieldCenterPos = new Vector3(gameStateHandler.playingFieldObject.transform.position.x, gameStateHandler.playingFieldObject.transform.position.y, 
                    gameStateHandler.playingFieldObject.transform.position.z);
                playingFieldCenterPos.y += fieldHeight / 2;
                playingFieldCenterPos.x += fieldWidth / 2;
                cameraRig.transform.position = playingFieldCenterPos;
                playingFieldTransform.transform.localPosition = -playingFieldCenterPos;

                cameraCentered = true;
                float size;
                size = fieldHeight > fieldWidth ? fieldHeight : fieldWidth;
                distanceFromCamera = size * 0.5f / Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            }


            // To follow the camera a new position for the transform based on the camera must be found.
            // This is done by taking the position of the camera currently, and also the direction in which the camera is looking with forward.
            // Distance from camera pushes the playingfield further away from the origin of the camera.
            // The rotation is also updated to ensure the view is straight on.

            newPosition = playingFieldTransform.transform.position + playingFieldTransform.transform.forward * (distanceFromCamera * 1.69f);
            transform.position = newPosition;
            transform.rotation = mainCamera.transform.localRotation; 
        }
    }
}
