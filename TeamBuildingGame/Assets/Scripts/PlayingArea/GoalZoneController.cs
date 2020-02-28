using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
    enum FieldShape
    {
        Vertical,
        Horizontal
    }

    float[] xSizeArray, ySizeArray;
    float maxX, maxY, minX, minY;

    [SerializeField]
    GameObject playingField;

    [SerializeField]
    bool goalScored = false;
    bool goalMoved = false;

    float goalZoneEdgeLength;
    float xDifference, yDifference;
    float goalZoneMiddleOffset;

    public GameObject blueGoal, redGoal;

    Vector2 centerOfField, centerOfBlueGoal, centerOfRedGoal;
    FieldShape fieldShape;
    GoalZoneRenderer blueGoalMesh, redGoalMesh;
    Vector3 fieldAnchor1, fieldAnchor2, fieldAnchor3, fieldAnchor4;

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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (goalScored)
        {
            GenerateNewGoalZones();
        }
    }


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

    void CalculateGoalZoneSize()
    {
        //Find the shortest edge and base size of a percentage of that. The 20f defines 20%. 
        if (fieldShape == FieldShape.Vertical)
        {
            goalZoneEdgeLength = (20f / 100f) * xDifference;
        }
        else
        {
            goalZoneEdgeLength = (20f / 100f) * yDifference;
        }

        goalZoneMiddleOffset = goalZoneEdgeLength / 2f;
    }


    void GenerateNewGoalZones()
    {
        float randomYBlue = 0;
        float randomXBlue = 0;
        Vector3[] anchors;

        if (!goalMoved)
        {
            if (fieldShape == FieldShape.Vertical)
            {
                //VerticalField - if blue center is > then field center it means blue is on top and should be swapped
                if (centerOfBlueGoal.y > centerOfField.y)
                {
                    //Goal zone middle offset ensures the goal does not have anchors that cross the middle, goal zone edge length ensures goals spawn a bit away from the middle line, and not on top of it.
                    randomYBlue = Random.Range(minY + goalZoneMiddleOffset, centerOfField.y - goalZoneMiddleOffset - goalZoneEdgeLength);
                    randomXBlue = Random.Range(minX + goalZoneMiddleOffset, maxX - goalZoneMiddleOffset);
                    centerOfBlueGoal = new Vector2(randomXBlue, randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfBlueGoal);

                    blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

                    // Red needs to mirror so we use the offset for the blue goal.
                    centerOfRedGoal = new Vector2(maxX - randomXBlue, maxY - randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfRedGoal);
                    redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

                }
                else
                {
                    randomYBlue = Random.Range(centerOfField.y + goalZoneMiddleOffset + goalZoneEdgeLength, maxY - goalZoneMiddleOffset);
                    randomXBlue = Random.Range(minX + goalZoneMiddleOffset, maxX - goalZoneMiddleOffset);
                    centerOfBlueGoal = new Vector2(randomXBlue, randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfBlueGoal);
                    blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

                    // Red needs to mirror so we use the offset for the blue goal.
                    centerOfRedGoal = new Vector2(maxX - randomXBlue, maxY - randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfRedGoal);
                    redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);
                    goalMoved = true;
                }
            }
            else
            {
                // Horizontal field - if blue center is < x then blue is on the left
                if (centerOfBlueGoal.x < centerOfField.x)
                {
                    randomXBlue = Random.Range(centerOfField.x + goalZoneMiddleOffset + goalZoneEdgeLength, maxX - goalZoneMiddleOffset);
                    randomYBlue = Random.Range(minY + goalZoneMiddleOffset, maxY - goalZoneMiddleOffset);

                    
                    centerOfBlueGoal = new Vector2(blueX, blueY);
                    anchors = DefineAnchorsForGoal(centerOfBlueGoal);

                    blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

                    // Red needs to mirror so we use the offset for the blue goal.
                    centerOfRedGoal = new Vector2(maxX - blueX, maxY - blueY);
                    anchors = DefineAnchorsForGoal(centerOfRedGoal);
                    redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);


                }
                else
                {
                    randomXBlue = Random.Range(minX + goalZoneMiddleOffset, centerOfField.x - goalZoneMiddleOffset - goalZoneEdgeLength);
                    randomYBlue = Random.Range(minY + goalZoneMiddleOffset, maxY - goalZoneMiddleOffset);
                    centerOfBlueGoal = new Vector2(randomXBlue, randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfBlueGoal);
                    blueGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);

                    // Red needs to mirror so we use the offset for the blue goal.
                    centerOfRedGoal = new Vector2(maxX - randomXBlue, maxY - randomYBlue);
                    anchors = DefineAnchorsForGoal(centerOfRedGoal);
                    redGoalMesh.MakeMeshData(anchors[0], anchors[1], anchors[2], anchors[3]);
                    goalMoved = true;
                }
            }
        }
    }

    Vector3[] DefineAnchorsForGoal(Vector2 midPointForGoal)
    {
        Vector3[] anchorArray = new Vector3[4];
        anchorArray[0] = new Vector3(midPointForGoal.x - goalZoneMiddleOffset, midPointForGoal.y - goalZoneMiddleOffset, 1f);
        anchorArray[1] = new Vector3(midPointForGoal.x - goalZoneMiddleOffset, midPointForGoal.y + goalZoneMiddleOffset, 1f);
        anchorArray[2] = new Vector3(midPointForGoal.x + goalZoneMiddleOffset, midPointForGoal.y - goalZoneMiddleOffset, 1f);
        anchorArray[3] = new Vector3(midPointForGoal.x + goalZoneMiddleOffset, midPointForGoal.y + goalZoneMiddleOffset, 1f);

        return anchorArray;
    }

    void SpawnVerticalGoals()
    {
        Vector3 anchor1, anchor2, anchor3, anchor4;
        anchor1 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
        anchor2 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY, 1f);
        anchor3 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
        anchor4 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY, 1f);

        centerOfBlueGoal = new Vector2((anchor1.x + anchor4.x) / 2, (anchor1.y + anchor4.y) / 2);

        blueGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);

        anchor1 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY, 1f);
        anchor2 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);
        anchor3 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY, 1f);
        anchor4 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);

        redGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);

        centerOfRedGoal = new Vector2((anchor1.x + anchor4.x) / 2, (anchor1.y + anchor4.y) / 2);
    }

    void SpawnHorizontalGoals()
    {
        Vector3 anchor1, anchor2, anchor3, anchor4;
        anchor1 = new Vector3(minX, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchor2 = new Vector3(minX, (maxY / 2f) + goalZoneMiddleOffset, 1f);
        anchor3 = new Vector3(minX + goalZoneEdgeLength, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchor4 = new Vector3(minX + goalZoneEdgeLength, (maxY / 2f) + goalZoneMiddleOffset, 1f);

        blueGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);

        anchor1 = new Vector3(maxX - goalZoneEdgeLength, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchor2 = new Vector3(maxX - goalZoneEdgeLength, (maxY / 2f) + goalZoneMiddleOffset, 1f);
        anchor3 = new Vector3(maxX, (maxY / 2f) - goalZoneMiddleOffset, 1f);
        anchor4 = new Vector3(maxX, (maxY / 2f) + goalZoneMiddleOffset, 1f);

        redGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);
    }

    void SpawnMirroringGoals(float blueX, float blueY)
    {

    }
}
