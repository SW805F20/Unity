using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalZoneController : MonoBehaviour
{
    enum FieldShape
    {
        Rectangle,
        Trapezoid,
        Parallelogram,
        Square
    }

    Mesh mesh;
    Vector3 fieldAnchor1, fieldAnchor2, fieldAnchor3, fieldAnchor4;

    float[] xSizeArray, ySizeArray;
    float maxX, maxY, minX, minY;
    float[] uniqueXSizes, uniqueYSizes;
    [SerializeField]
    GameObject playingField;
    bool goalScored = false;
    FieldShape fieldShape;
    float goalZoneEdgeLength;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    public GameObject blueGoal, redGoal;
    float xDifference, yDifference;
    float goalZoneMiddleOffset;
    Transform goalHandler;

    void Awake()
    {
        goalHandler = this.gameObject.transform;

        fieldAnchor1 = playingField.GetComponent<FieldGenerator>().anchor1;
        fieldAnchor2 = playingField.GetComponent<FieldGenerator>().anchor2;
        fieldAnchor3 = playingField.GetComponent<FieldGenerator>().anchor3;
        fieldAnchor4 = playingField.GetComponent<FieldGenerator>().anchor4;

        

        xSizeArray = new float[] { fieldAnchor1.x, fieldAnchor2.x, fieldAnchor3.x, fieldAnchor4.x};
        ySizeArray = new float[] { fieldAnchor1.y, fieldAnchor2.y, fieldAnchor3.y, fieldAnchor4.y };

       
    }

    // Start is called before the first frame update
    void Start()
    {
        uniqueXSizes = xSizeArray.Distinct().ToArray();
        uniqueYSizes = ySizeArray.Distinct().ToArray();
        maxX = Mathf.Max(xSizeArray);
        maxY = Mathf.Max(ySizeArray);
        minX = Mathf.Min(xSizeArray);
        minY = Mathf.Min(ySizeArray);

        GetXAndYDifferences();

        CalculateGoalZoneSize();

        CalculateShape();
       
  
        SpawnFirstSetOfGoalZones();

    }

    // Update is called once per frame
    void Update()
    {
        if(goalScored)
        {
            switch (fieldShape)
            {
                case FieldShape.Rectangle:
                    SpawnRandomZonesRectangle();
                    break;
                case FieldShape.Trapezoid:
                    break;
                case FieldShape.Parallelogram:
                    break;
                case FieldShape.Square:
                    break;
            }
        }


    }

    void CalculateShape()
    {
        //If there are only two distinct values in the y array it means that it must be a rectangle.
        if(ySizeArray.Distinct().Count() == 2 && xSizeArray.Distinct().Count() == 2)
        {
            fieldShape = FieldShape.Rectangle;
        }
        // We know the points are clockwise. Bottom left is 0, top left is 1. We can then look at the length of the edge. If they are equal, it must be a parralelogram.
        else if (xSizeArray[1] - xSizeArray[0] == xSizeArray[3] - xSizeArray[2])
        {
            if(ySizeArray[1] - ySizeArray[0] == ySizeArray[3] - ySizeArray[2])
            {
                fieldShape = FieldShape.Parallelogram;
            }
            else
            {
                fieldShape = FieldShape.Trapezoid;
            }
        }
        else if (ySizeArray[1] - ySizeArray[0] == ySizeArray[3] - ySizeArray[2])
        {
            if (xSizeArray[1] - xSizeArray[0] == xSizeArray[3] - xSizeArray[2])
            {
                fieldShape = FieldShape.Parallelogram;
            }
            else
            {
                fieldShape = FieldShape.Trapezoid;
            }
        }
        else
        {
            fieldShape = FieldShape.Square;
        }

    }

    void GetXAndYDifferences()
    {
        xDifference = maxX - minX;
        yDifference = maxY - minY;
    }

    void SpawnFirstSetOfGoalZones()
    {
        switch (fieldShape)
        {
            case FieldShape.Rectangle:
                SpawnOppositeGoalsRectangle();
                break;
            case FieldShape.Trapezoid:
                break;
            case FieldShape.Parallelogram:
                break;
            case FieldShape.Square:
                break;
        }
    }

    void CalculateGoalZoneSize()
    {
        //Find the shortest edge and base size of a percentage of that
        if (xDifference < yDifference)
        {
            goalZoneEdgeLength =  (20f / 100f) * xDifference;
        }
        else
        {
            goalZoneEdgeLength = (20f / 100f) * yDifference;
        }
        goalZoneMiddleOffset = goalZoneEdgeLength / 2f;
    }

    void SpawnOppositeGoalsRectangle()
    {
        // The sides to spawn goals are picked based on which edges are longer.
        if (yDifference > xDifference)
        {
            Vector3 anchor1, anchor2, anchor3, anchor4;
            anchor1 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
            anchor2 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), maxY, 1f);
            anchor3 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY - goalZoneEdgeLength, 1f);
            anchor4 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), maxY, 1f);

            blueGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);

            anchor1 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY, 1f);
            anchor2 = new Vector3(((maxX / 2f) - goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);
            anchor3 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY, 1f);
            anchor4 = new Vector3(((maxX / 2f) + goalZoneMiddleOffset), minY + goalZoneEdgeLength, 1f);

            redGoal.GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);
        }
        else
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

    }

    void SpawnRandomZonesRectangle()
    {
        
        if (yDifference > xDifference)
        {
            float[] randoms = new float[2];
            randoms = RandomValuesForSpawnPoints(minY, maxY);
            DefineAnchorsRandomRectangle(randoms);
        } 
        else
        {
            float[] randoms = new float[2];
            randoms = RandomValuesForSpawnPoints(minX, maxX);
            DefineAnchorsRandomRectangle(randoms);
        }
       

    }

    float[] RandomValuesForSpawnPoints(float min, float max)
    {
        float[] randoms = new float[2];
        do
        {
            for (int i = 0; i < randoms.Length; i++)
            {
                randoms[i] = Random.Range((min + goalZoneEdgeLength / 2f), (max - goalZoneMiddleOffset));
            }
            randoms.OrderBy(rand => rand);
        } while (randoms[0] + goalZoneEdgeLength >= randoms[1]);

        return randoms;
    }

    void DefineAnchorsRandomRectangle(float[] randoms)
    {
        for(int i = 0; i < randoms.Length; i ++)
        {
            Vector3 anchor1, anchor2, anchor3, anchor4;
            anchor1 = new Vector3((randoms[i] - goalZoneMiddleOffset), (randoms[i] - goalZoneMiddleOffset), 1f);
            anchor2 = new Vector3((randoms[i] - goalZoneMiddleOffset), (randoms[i] + goalZoneMiddleOffset), 1f);
            anchor3 = new Vector3((randoms[i] + goalZoneMiddleOffset), (randoms[i] - goalZoneMiddleOffset), 1f);
            anchor4 = new Vector3((randoms[i] + goalZoneMiddleOffset), (randoms[i] + goalZoneMiddleOffset), 1f);

            goalHandler.GetChild(i).GetComponent<GoalZoneRenderer>().MakeMeshData(anchor1, anchor2, anchor3, anchor4);
        }
    }


}
