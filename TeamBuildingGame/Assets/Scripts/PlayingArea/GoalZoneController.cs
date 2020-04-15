using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
    float[] xSizeArray, ySizeArray; // Arrays for all sizes on both a and y axis, used to fid maximum and minimum values
    float goalSizeOffset;

    public GameObject blueGoal, redGoal;
    public float goalLengthPercentage; // The percentage size of the edges of a goal. The percentage is based on the shortest edge.
    public GameObject playingField;
    public GoalZoneRenderer blueGoalMesh, redGoalMesh;

    // x and y differences are the difference between the maximum and minimum values. Goalzone edge length is how long each edge is. The middle offset is the distance between the midpoint and an edge.
    [HideInInspector]
    public float maxX, maxY, minX, minY, xDifference, yDifference, goalZoneEdgeLength, goalZoneMiddleOffset;
    
    [SerializeField]
    bool goalScored = false;  

    [HideInInspector]
    public Vector2 centerOfField, centerOfBlueGoal, centerOfRedGoal;

    [HideInInspector]
    public Vector3 fieldAnchor1, fieldAnchor2, fieldAnchor3, fieldAnchor4;

    /// <summary> This method uses Unity's awake method to define starting requirements.
    ///    It gets the anchors from the playingField game object, and uses these to create arrays of their positions.
    ///    Finally it gets the rendering components of the goalzone child game objects.</summary>
    void Awake()
    {
        fieldAnchor1 = playingField.GetComponent<FieldGenerator>().anchor1;
        fieldAnchor2 = playingField.GetComponent<FieldGenerator>().anchor2;
        fieldAnchor3 = playingField.GetComponent<FieldGenerator>().anchor3;
        fieldAnchor4 = playingField.GetComponent<FieldGenerator>().anchor4;

        xSizeArray = new float[] { fieldAnchor1.x, fieldAnchor2.x, fieldAnchor3.x, fieldAnchor4.x };
        ySizeArray = new float[] { fieldAnchor1.y, fieldAnchor2.y, fieldAnchor3.y, fieldAnchor4.y };

        blueGoalMesh = blueGoal.GetComponent<GoalZoneRenderer>();
        redGoalMesh = redGoal.GetComponent<GoalZoneRenderer>();
    }

    /// <summary> This method uses Unity's start method to define maximum and minimum coordinate values.
    ///    It then defines the center of the field, and calls the necessary methods to create the starting goalzones.</summary>
    void Start()
    {
        var allSides = new float[] { fieldAnchor2.y - fieldAnchor1.y, fieldAnchor3.x - fieldAnchor2.x, fieldAnchor3.y - fieldAnchor4.y, fieldAnchor4.x - fieldAnchor1.x };
        float smallestSide = Mathf.Min(allSides);
        goalSizeOffset = (smallestSide / 100 * goalLengthPercentage) / 2;
    }

    /// <summary> This method uses Unity's update method to continually check for goals being scored.
    ///    If a goal is scored it should make new goalzones and flip sides.</summary>
    void Update()
    {

    }

    /// <summary>
    /// Spawns the goals 
    /// </summary>
    /// <param name="redGoalCenter">Center of the red goal</param>
    /// <param name="blueGoalCenter">Center of the blue goal</param>
    public void SpawnGoals(Vector2 redGoalCenter, Vector2 blueGoalCenter)
    {
        Vector3[] redCorners = CreateGoalCorners(redGoalCenter);
        Vector3[] blueCorners = CreateGoalCorners(blueGoalCenter);

        redGoalMesh.MakeMeshData(redCorners[0], redCorners[1], redCorners[2], redCorners[3]);
        blueGoalMesh.MakeMeshData(blueCorners[0], blueCorners[1], blueCorners[2], blueCorners[3]);
    }

    /// <summary>
    /// Creates the corners of a goal based of the center of the goal and the given size of the goal
    /// </summary>
    /// <param name="goalCenter">The center of a given goal</param>
    /// <returns></returns>
    private Vector3[] CreateGoalCorners(Vector2 goalCenter)
    {
        float zAxisOffset = 1;
        Vector3[] corners = new Vector3[4];
        // corners[0] is the lower left corner, they then proceed clockwise
        corners[0] = new Vector3(goalCenter.x - goalSizeOffset, goalCenter.y - goalSizeOffset, zAxisOffset);
        corners[1] = new Vector3(goalCenter.x - goalSizeOffset, goalCenter.y + goalSizeOffset, zAxisOffset);
        corners[2] = new Vector3(goalCenter.x + goalSizeOffset, goalCenter.y + goalSizeOffset, zAxisOffset);
        corners[3] = new Vector3(goalCenter.x + goalSizeOffset, goalCenter.y - goalSizeOffset, zAxisOffset);
        return corners;
    }
}
