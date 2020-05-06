﻿using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        gameStateHandler.ball.transform.localPosition = new Vector3(gameStateHandler.ballPosition.x, gameStateHandler.ballPosition.y, -6);
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            gameStateHandler.players[i].transform.localPosition = new Vector3(gameStateHandler.playerPositions[i].x, gameStateHandler.playerPositions[i].y, -6);
        }
    }
}
