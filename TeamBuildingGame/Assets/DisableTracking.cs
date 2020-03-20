using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class DisableTracking : MonoBehaviour
{
    private bool cameraReset = false;
    private float xMax, yMax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchTo2D());
    }

    // Update is called once per frame
    void Update()
    {
        xMax = gameObject.GetComponentInParent<MoveCameraToMiddle>().xMax;
        yMax = gameObject.GetComponentInParent<MoveCameraToMiddle>().yMax;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = xMax / yMax;
        if (cameraReset)
        {
            if (screenRatio >= targetRatio)
            {
                this.GetComponent<Camera>().orthographicSize = yMax / 2f;
            }
            else
            {
                float sizeDifference = targetRatio / screenRatio;
                this.GetComponent<Camera>().orthographicSize = yMax / 2f * sizeDifference;
            }
        }
    }

    // Call via `StartCoroutine(SwitchTo2D())` from your code. Or, use
    // `yield SwitchTo2D()` if calling from inside another coroutine.
    IEnumerator SwitchTo2D()
    {
        // Empty string loads the "None" device.
        XRSettings.LoadDeviceByName("");

        // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
        yield return null;

        ResetCameras();
    }

    void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {

                // Reset local position.
                // Only required if you change the camera's local position while in 2D mode.
                cam.transform.localPosition = Vector3.zero;

                // Reset local rotation.
                // Only required if you change the camera's local rotation while in 2D mode.
                cam.transform.localRotation = Quaternion.identity;
            }
        }

        cameraReset = true;
    }
}
