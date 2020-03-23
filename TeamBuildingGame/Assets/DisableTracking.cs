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
        XRDevice.DisableAutoXRCameraTracking(this.GetComponent<Camera>(), true);
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
}
