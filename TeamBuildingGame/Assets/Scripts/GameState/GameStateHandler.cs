using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public byte playerCount = 0;
    public byte goalsToWin = 0;
    public int teamCount = 2;
    public Vector3[] prevPlayerPositions;
    public Vector3[] newPlayerPositions;
    public float[] journeyLength;
    public float journeyLengthBall;
    public float[] timeAtLastUpdate;
    public float timeAtLastUpdateBall;
    public Vector3 anchor1, anchor2, anchor3, anchor4;
    public Vector3 prevBallPosition;
    public Vector3 newBallPosition;
    public float[] playerSpeed;
    public float ballSpeed = 5.0f;
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
    public byte team1Score = 0;
    public byte team2Score = 0;
    public int myPlayerId;
    public string myPlayerTag;
}
