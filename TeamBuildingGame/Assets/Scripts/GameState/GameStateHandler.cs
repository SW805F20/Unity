using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public int playerCount;
    public int teamCount = 2;
    public Vector2[] playerPositions;
    public Vector3 anchor1, anchor2, anchor3, anchor4;
    public Vector2 ballPosition;
    public GameObject playingFieldContainer;
    public GameObject playingFieldObject;
    public GameObject team1Prefab;
    public GameObject team2Prefab;
    public GameObject ballPrefab;
    public GameObject ball;
    public List<GameObject> players;
    public int goalSize;
    public GameObject GoalZoneController;
    public Vector2 redGoal;
    public Vector2 blueGoal;
    public int goalCenterOffset;
    public GoalZoneController goalZoneControllerScript;
    public FieldGenerator fieldGeneratorScript;
    public int myPlayerId;
    public string myPlayerTag;
}
