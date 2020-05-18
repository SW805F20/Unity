using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectMover : MonoBehaviour
{
    GameObject client;
    GameStateHandler gameStateHandler;

    UDPClient clientData;

    void Awake() {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        gameStateHandler.playingFieldObject = GameObject.Find("PlayingField");
    }

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CLIENT");
        clientData = client.GetComponent<UDPClient>();

        gameStateHandler.ball = Instantiate(gameStateHandler.ballPrefab);
        gameStateHandler.ball.transform.SetParent(gameStateHandler.playingFieldObject.transform, false);

        gameStateHandler.players = new List<GameObject>();
        GameObject playerObject;
   
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            if (i < gameStateHandler.playerCount / 2)
            {
                playerObject = Instantiate(gameStateHandler.team1Prefab);
            }
            else
            {
                playerObject = Instantiate(gameStateHandler.team2Prefab);
            }
            playerObject.name = "player" + i.ToString();
            playerObject.transform.SetParent(gameStateHandler.playingFieldObject.transform, false);
            gameStateHandler.players.Add(playerObject);

        }
        // With Unity, each individual prefab instance must be colored
        // If only one player is colored, multiple instance will get the color
        // The client's player is colored red, the others white
        foreach(var player in gameStateHandler.players)
        {
            if(player.name == "player" + (gameStateHandler.myPlayerId - 1).ToString())
            {
                player.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                player.GetComponent<SpriteRenderer>().color = Color.white;
            }

        }

        // Set the players speed for lerping
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            gameStateHandler.playerSpeed[i] = 60.0f;
        }
        // Set ball speed
        gameStateHandler.ballSpeed = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStateHandler.journeyLengthBall != 0)
        {
            // Distance moved equals elapsed time times speed
            float distCoveredBall = (float)(DateTime.UtcNow - gameStateHandler.timeAtLastUpdateBall).TotalSeconds * gameStateHandler.ballSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourneyBall = distCoveredBall / gameStateHandler.journeyLengthBall;

            // Set our position as a fraction of the distance between the markers.
            var vector2CoordinatesBall = Vector2.Lerp(gameStateHandler.prevBallPosition, gameStateHandler.newBallPosition, fractionOfJourneyBall);
            gameStateHandler.ball.transform.localPosition = new Vector3(vector2CoordinatesBall.x, vector2CoordinatesBall.y, -7);
        }

        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            if(gameStateHandler.journeyLength[i] != 0)
            {
                float distCovered = (float)(DateTime.UtcNow - gameStateHandler.timeAtLastUpdateBall).TotalSeconds * gameStateHandler.playerSpeed[i];

                float fractionOfJourney = distCovered / gameStateHandler.journeyLength[i];

                var vector2Coordinates = Vector2.Lerp(gameStateHandler.prevPlayerPositions[i], gameStateHandler.newPlayerPositions[i], fractionOfJourney);
                gameStateHandler.players[i].transform.localPosition = new Vector3(vector2Coordinates.x, vector2Coordinates.y, -6);
            }

        }
    }
}
