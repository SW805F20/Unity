using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{

    public GameObject blueGoal, redGoal;
    public GoalZoneRenderer blueGoalMesh, redGoalMesh;

    [SerializeField]
    bool goalScored = false;  

    [HideInInspector]
    public Vector2 centerOfBlueGoal, centerOfRedGoal;


    /// <summary> This method uses Unity's awake method to define starting requirements.
    ///    It gets the anchors from the playingField game object, and uses these to create arrays of their positions.
    ///    Finally it gets the rendering components of the goalzone child game objects.</summary>
    void Awake()
    {
        blueGoalMesh = blueGoal.GetComponent<GoalZoneRenderer>();
        redGoalMesh = redGoal.GetComponent<GoalZoneRenderer>();
    }

    /// <summary>
    /// Spawns red goal
    /// </summary>
    /// <param name="redGoalCenter">Center of the red goal</param>
    public void SpawnRedGoal(Vector2 redGoalCenter, int goalZoneCenterOffset)
    {
        Vector3[] redCorners = CreateGoalCorners(redGoalCenter, goalZoneCenterOffset);

        redGoalMesh.MakeMeshData(redCorners);
    }


    /// <summary>
    /// Spawns the goals 
    /// </summary>
    /// <param name="redGoalCenter">Center of the red goal</param>
    /// <param name="blueGoalCenter">Center of the blue goal</param>
    public void SpawnBlueGoal(Vector2 blueGoalCenter, int goalZoneCenterOffset)
    {
        Vector3[] blueCorners = CreateGoalCorners(blueGoalCenter, goalZoneCenterOffset);

        blueGoalMesh.MakeMeshData(blueCorners);
    }

    /// <summary>
    /// Creates the corners of a goal based of the center of the goal and the given size of the goal
    /// </summary>
    /// <param name="goalCenter">The center of a given goal</param>
    /// <returns></returns>
    private Vector3[] CreateGoalCorners(Vector2 goalCenter, int goalSizeOffset)
    {
        float zAxisOffset = 0;

        Vector3[] corners = new Vector3[4];
        // corners[0] is the lower left corner, they then proceed clockwise
        corners[0] = new Vector3(goalCenter.x - goalSizeOffset, goalCenter.y - goalSizeOffset, zAxisOffset);
        corners[1] = new Vector3(goalCenter.x - goalSizeOffset, goalCenter.y + goalSizeOffset, zAxisOffset);
        corners[2] = new Vector3(goalCenter.x + goalSizeOffset, goalCenter.y + goalSizeOffset, zAxisOffset);
        corners[3] = new Vector3(goalCenter.x + goalSizeOffset, goalCenter.y - goalSizeOffset, zAxisOffset);
        return corners;
    }
}
