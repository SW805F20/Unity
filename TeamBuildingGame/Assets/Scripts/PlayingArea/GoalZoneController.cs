﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
    // If the x-axis has a higher maximum value than the y-axis, it means the horizontal edges of the field are longer, and this it is a horizontal playingfield.
    enum FieldShape
    {
        Vertical,
        Horizontal
    }

    // Arrays for all sizes on both a and y axis, used to fid maximum and minimum values
    float[] xSizeArray, ySizeArray;

    // x and y differences are the difference between the maximum and minimum values. Goalzone edge length is how long each edge is. The middle offset is the distance between the midpoint and an edge.
    [HideInInspector]
    public float maxX, maxY, minX, minY, xDifference, yDifference, goalZoneEdgeLength, goalZoneMiddleOffset;
    // The percentage size of the edges of a goal. The percentage is based on the shortest edge.
    public float goalLengthPercentage;

    public GameObject playingField;

    [SerializeField]
    bool goalScored = false;

    public GameObject blueGoal, redGoal;

    [HideInInspector]
    public Vector2 centerOfField, centerOfBlueGoal, centerOfRedGoal;

    FieldShape fieldShape;

    public GoalZoneRenderer blueGoalMesh, redGoalMesh;

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
        maxX = Mathf.Max(xSizeArray);
        maxY = Mathf.Max(ySizeArray);
        minX = Mathf.Min(xSizeArray);
        minY = Mathf.Min(ySizeArray);

        centerOfField = new Vector2(maxX / 2, maxY / 2);

        DefineFieldShape();

        CalculateGoalZoneSize();

        SpawnFirstSetOfGoalZones();
    }

    /// <summary> This method uses Unity's update method to continually check for goals being scored.
    ///    If a goal is scored it should make new goalzones and flip sides.</summary>
    void Update()
    {
        if (goalScored)
        {
            goalScored = false;
            GenerateNewGoalZones();
        }
    }

    /// <summary> This method defines the field shape based on the differences in maximum and minimum x and y values.
    ///    If the difference in maximum and minimum x is smaller than the difference in y, it is a vertical field.</summary>
    void DefineFieldShape()
    {
        xDifference = maxX - minX;
        yDifference = maxY - minY;

        if (xDifference < yDifference)
        {
            fieldShape = FieldShape.Vertical;
        }
        else
        {
            fieldShape = FieldShape.Horizontal;
        }
    }

    /// <summary> This method simply calls the corresponding method to create the initial goalzones
    ///    based on whether or not the field is vertical or horizontal.</summary>
    void SpawnFirstSetOfGoalZones()
    {
        // The sides to spawn goals are picked based on which edges are longer.
        if (fieldShape == FieldShape.Vertical)
        {
            SpawnVerticalGoals();
        }
        else
        {
            SpawnHorizontalGoals();
        }
    }

    /// <summary> This method calculates the size of an edge of the goal zone, based on the shortest edge.
    ///    The number being divided by 100 represents the percentage size of the goal zone edge.
    ///    The offset needed when defining anchors based on the center of a zone is then defined as half the length of the edges.</summary>
    void CalculateGoalZoneSize()
    {
        //Find the shortest edge and base size of a percentage of that. Goal zone percentage defines how large the edge should be based on a percentage. 
        if (fieldShape == FieldShape.Vertical)
        {
            goalZoneEdgeLength = (goalLengthPercentage / 100f) * xDifference;
        }
        else
        {
            goalZoneEdgeLength = (goalLengthPercentage / 100f) * yDifference;
        }

        goalZoneMiddleOffset = goalZoneEdgeLength / 2f;
    }

    /// <summary> This method accounts for the different possibilities for new goal zones to create a range in which to
    ///    random a number for the blue goal zone. Another method then uses the numbers generated from the proper range. </summary>
    void GenerateNewGoalZones()
    {
        float randomYBlue = 0;
        float randomXBlue = 0;
        if (fieldShape == FieldShape.Vertical)
        {
            //VerticalField - if blue center is > then field center it means blue is on top. Random ranges are defined to ensure it swaps sides.
            if (centerOfBlueGoal.y > centerOfField.y)
            {
                //Goal zone middle offset ensures the goal does not have anchors that cross the middle, goal zone edge length ensures goals spawn a bit away from the middle line, and not on top of it.
                randomYBlue = Random.Range(minY + goalZoneMiddleOffset, centerOfField.y - goalZoneMiddleOffset - goalZoneEdgeLength);
                randomXBlue = Random.Range(minX + goalZoneMiddleOffset, maxX - goalZoneMiddleOffset);
            }
            else
            {
                randomYBlue = Random.Range(centerOfField.y + goalZoneMiddleOffset + goalZoneEdgeLength, maxY - goalZoneMiddleOffset);
                randomXBlue = Random.Range(minX + goalZoneMiddleOffset, maxX - goalZoneMiddleOffset);
            }
        }
        else
        {
            // Horizontal field - if blue center is < x then blue is on the left
            if (centerOfBlueGoal.x < centerOfField.x)
            {
                randomXBlue = Random.Range(centerOfField.x + goalZoneMiddleOffset + goalZoneEdgeLength, maxX - goalZoneMiddleOffset);
                randomYBlue = Random.Range(minY + goalZoneMiddleOffset, maxY - goalZoneMiddleOffset);
            }
            else
            {
                randomXBlue = Random.Range(minX + goalZoneMiddleOffset, centerOfField.x - goalZoneMiddleOffset - goalZoneEdgeLength);
                randomYBlue = Random.Range(minY + goalZoneMiddleOffset, maxY - goalZoneMiddleOffset);
            }
        }

        SpawnMirroringGoals(randomXBlue, randomYBlue);
    }

    /// <summary>This method defines the anchors of the goalzones through its center,
    ///    by using the offset needed, which is half of the edge length of the goal zone.</summary>
    /// <param name="midPointForGoal">The center of the goalzone from which to build anchors.</param>
    public Vector3[] DefineAnchorsForGoal(Vector2 midPointForGoal)
    {
        Vector3[] anchorArray = new Vector3[4];
        anchorArray[0] = new Vector3(midPointForGoal.x - goalZoneMiddleOffset, midPointForGoal.y - goalZoneMiddleOffset, 1f);
        anchorArray[1] = new Vector3(midPointForGoal.x - goalZoneMiddleOffset, midPointForGoal.y + goalZoneMiddleOffset, 1f);
        anchorArray[2] = new Vector3(midPointForGoal.x + goalZoneMiddleOffset, midPointForGoal.y - goalZoneMiddleOffset, 1f);
        anchorArray[3] = new Vector3(midPointForGoal.x + goalZoneMiddleOffset, midPointForGoal.y + goalZoneMiddleOffset, 1f);

        return anchorArray;
    }

    /// <summary>This method defines the center of the goalzones, calls another function to define the anchors,
    ///    then invokes the method on the child mesh that renders both zones.</summary>
    /// <param name="blueX">The x-coordinate for the random midpoint for the blue goal.</param>
    /// <param name="blueY">The y-coordinate for the random midpoint for the blue goal.</param>
    void SpawnMirroringGoals(float blueX, float blueY)
    {
        Vector3[] anchors;
        centerOfBlueGoal = new Vector2(blueX, blueY);
        anchors = DefineAnchorsForGoal(centerOfBlueGoal);

        blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

        // Red needs to mirror so we use the offset from the blue goal.
        centerOfRedGoal = new Vector2(maxX - blueX, maxY - blueY);
        anchors = DefineAnchorsForGoal(centerOfRedGoal);
        redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);
    }

    /// <summary>This method defines the anchors for the starting goalzones on a vertical field,
    ///    then invokes the method on the child mesh that renders it.</summary>
    public void SpawnVerticalGoals()
    {
        Vector3[] anchors = new Vector3[4];
        anchors = DefineVerticalBlueGoal();
        
        blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

        anchors = DefineVerticalRedGoal();
        redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);
    }

    /// <summary>This method defines the anchors for the starting goalzone on the top half of the vertical field.</summary>
    public Vector3[] DefineVerticalBlueGoal()
    {
        Vector3[] anchors = new Vector3[4];
        anchors[0] = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
        anchors[1] = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY, 1f);
        anchors[2] = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
        anchors[3] = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY, 1f);

        return anchors;
    }

    /// <summary>This method defines the anchors for the starting goalzone on the bottom half of the vertical field.</summary>
    public Vector3[] DefineVerticalRedGoal()
    {
        Vector3[] anchors = new Vector3[4];
        anchors[0] = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY, 1f);
        anchors[1] = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);
        anchors[2] = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY, 1f);
        anchors[3] = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);

        return anchors;
    }

    /// <summary>This method defines the anchors for the starting goalzones on a horizontal field,
    ///    then invokes the method on the child mesh that renders it.</summary>
    public void SpawnHorizontalGoals()
    {
        Vector3[] anchors = new Vector3[4];
        anchors = DefineHorizontalBlueGoal();
        blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

        anchors = DefineHorizontalRedGoal();
        redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);
    }

    /// <summary>This method defines the anchors for the starting goalzone on the left half of the horizontal field.</summary>
    public Vector3[] DefineHorizontalBlueGoal()
    {
        Vector3[] anchors = new Vector3[4];
        anchors[0] = new Vector3(minX, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchors[1] = new Vector3(minX, (maxY / 2f) + goalZoneMiddleOffset, 1f);
        anchors[2] = new Vector3(minX + goalZoneEdgeLength, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchors[3] = new Vector3(minX + goalZoneEdgeLength, (maxY / 2f) + goalZoneMiddleOffset, 1f);

        return anchors;
    }

    /// <summary>This method defines the anchors for the starting goalzone on the right half of the horizontal field.</summary>
    public Vector3[] DefineHorizontalRedGoal()
    {
        Vector3[] anchors = new Vector3[4];
        anchors[0] = new Vector3(maxX - goalZoneEdgeLength, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchors[1] = new Vector3(maxX - goalZoneEdgeLength, (maxY / 2f) + goalZoneMiddleOffset, 1f);
        anchors[2] = new Vector3(maxX, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchors[3] = new Vector3(maxX, (maxY / 2f) + goalZoneMiddleOffset, 1f);

        return anchors;
    }
}