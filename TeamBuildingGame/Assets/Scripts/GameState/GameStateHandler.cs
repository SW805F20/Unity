using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public int playerCount;
    public int teamCount = 2;
    public Vector2[] playerPositions;
    public Vector2 ballPosition;
    public GameObject playingFieldContainer;
    public GameObject playingFieldObject;
    public GameObject team1Prefab;
    public GameObject team2Prefab;
    public GameObject ballPrefab;
    public GameObject ball;
    public GameObject[] players;
    public int goalSize;
    public GameObject GoalZoneController;
    public Vector2[] goalPositions;
    public GoalZoneController goalZoneControllerScript;
    public FieldGenerator fieldGeneratorScript;
    public byte team1Score = 0;
    public byte team2Score = 0;
}
