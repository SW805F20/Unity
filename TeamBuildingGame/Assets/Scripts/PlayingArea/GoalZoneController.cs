using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{

    public GameObject blueGoal, redGoal;
    public GoalZoneRenderer blueGoalMesh, redGoalMesh;
    private GameStateHandler gameStateHandler;

    /// <summary> This method uses Unity's awake method to define starting requirements.
    ///    It gets the anchors from the playingField game object, and uses these to create arrays of their positions.
    ///    Finally it gets the rendering components of the goalzone child game objects.</summary>
    void Awake()
    {
        blueGoalMesh = blueGoal.GetComponent<GoalZoneRenderer>();
        redGoalMesh = redGoal.GetComponent<GoalZoneRenderer>();
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        gameStateHandler.goalZoneControllerScript = gameObject.GetComponent<GoalZoneController>();
        SpawnGoal(gameStateHandler.blueGoal, gameStateHandler.goalCenterOffset, 0);
        SpawnGoal(gameStateHandler.redGoal, gameStateHandler.goalCenterOffset, 1);
    }

    /// <summary>
    /// Spawns the goals 
    /// </summary>
    /// <param name="goalCenter">Center of the blue goal</param>
    /// <param name="goalZoneCenterOffset">the length from the center of the goal to the edge</param>
    /// <param name="teamId">the id of the team</param>
    public void SpawnGoal(Vector2 goalCenter, int goalZoneCenterOffset, byte teamId)
    {

        Vector3[] goalCorners = CreateGoalCorners(goalCenter, goalZoneCenterOffset);
        if(teamId == 0)
        {
            blueGoalMesh.MakeMeshData(goalCorners);
        } 
        else if (teamId == 1)
        {
            redGoalMesh.MakeMeshData(goalCorners);
        }

    }

    /// <summary>
    /// Creates the corners of a goal based of the center of the goal and the given size of the goal
    /// </summary>
    /// <param name="goalCenter">The center of a given goal</param>
    /// <param name="goalZoneCenterOffset">the length from the center of the goal to the edge</param>
    /// <returns></returns>
    private Vector3[] CreateGoalCorners(Vector2 goalCenter, int goalZoneCenterOffset)
    {
        float zAxisOffset = 0;

        Vector3[] corners = new Vector3[4];
        // corners[0] is the lower left corner, they then proceed clockwise
        corners[0] = new Vector3(goalCenter.x - goalZoneCenterOffset, goalCenter.y - goalZoneCenterOffset, zAxisOffset);
        corners[1] = new Vector3(goalCenter.x - goalZoneCenterOffset, goalCenter.y + goalZoneCenterOffset, zAxisOffset);
        corners[2] = new Vector3(goalCenter.x + goalZoneCenterOffset, goalCenter.y + goalZoneCenterOffset, zAxisOffset);
        corners[3] = new Vector3(goalCenter.x + goalZoneCenterOffset, goalCenter.y - goalZoneCenterOffset, zAxisOffset);
        return corners;
    }
}
