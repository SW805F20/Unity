using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = mainCamera.transform.localPosition;

        /*position.z = position.z = 10;
        position.y = position.y + 5;
        transform.localPosition = position;*/
        // transform.localRotation = mainCamera.transform.localRotation;

        // Vector3 resultingPosition = mainCamera.transform.position + mainCamera.transform.forward * 10;
        //transform.position = resultingPosition;

        Vector3 resultingPosition = mainCamera.transform.position + mainCamera.transform.forward * 10;
        transform.position = new Vector3(resultingPosition.x, resultingPosition.y, resultingPosition.z);
        transform.rotation = mainCamera.transform.localRotation;
    }
}
