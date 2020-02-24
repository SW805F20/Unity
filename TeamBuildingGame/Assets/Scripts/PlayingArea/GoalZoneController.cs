using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
    Mesh mesh;
    Vector3 fieldAnchor1, fieldAnchor2, fieldAnchor3, fieldAnchor4;
    [SerializeField]
    Vector3 anchor1, anchor2, anchor3, anchor4;
    float[] xSizeArray, ySizeArray;
    float maxX, maxY, minX, minY;
    [SerializeField]
    GameObject playingField;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        anchor1 = playingField.GetComponent<FieldGenerator>().anchor1;
        anchor2 = playingField.GetComponent<FieldGenerator>().anchor2;
        anchor3 = playingField.GetComponent<FieldGenerator>().anchor3;
        anchor4 = playingField.GetComponent<FieldGenerator>().anchor4;

        xSizeArray = new float[]{ anchor1.x, anchor2.x, anchor3.x, anchor4.x};
        ySizeArray = new float[] { anchor1.y, anchor2.y, anchor3.y, anchor4.y };
    }

    // Start is called before the first frame update
    void Start()
    {
        maxX = Mathf.Max(xSizeArray);
        maxY = Mathf.Max(ySizeArray);
        minX = Mathf.Min(xSizeArray);
        minY = Mathf.Min(ySizeArray);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
