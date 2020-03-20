using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCameraToMiddle : MonoBehaviour
{
    public FieldGenerator playingField;
    private List<float> xAnchors = new List<float>();
    private List<float> yAnchors = new List<float>();
    public float xMax, yMax;
    private float xMiddle, yMiddle;

    // Start is called before the first frame update
    void Start()
    {
        xAnchors.Add(playingField.anchor1.x);
        xAnchors.Add(playingField.anchor2.x);
        xAnchors.Add(playingField.anchor3.x);
        xAnchors.Add(playingField.anchor4.x);

        yAnchors.Add(playingField.anchor1.y);
        yAnchors.Add(playingField.anchor2.y);
        yAnchors.Add(playingField.anchor3.y);
        yAnchors.Add(playingField.anchor4.y);
        xMax = xAnchors.Max();
        yMax = yAnchors.Max();
        xMiddle = xMax / 2f;
        yMiddle = yMax / 2f;
        transform.position = new Vector3(xMiddle, yMiddle, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
