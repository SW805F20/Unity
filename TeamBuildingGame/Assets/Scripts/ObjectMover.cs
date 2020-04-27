using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    GameObject client;

    [SerializeField]
    GameObject gameState;
    GameStateHandler gameStateHandler;

    UDPClient clientData;

    void Awake() {
        gameStateHandler = gameState.GetComponent<GameStateHandler>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CLIENT");
        clientData = client.GetComponent<UDPClient>();

        gameStateHandler.ball = Instantiate(gameStateHandler.ballPrefab);
        gameStateHandler.players = new GameObject[gameStateHandler.playerCount];
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            if (i < gameStateHandler.playerCount / 2)
            {
                gameStateHandler.players[i] = Instantiate(gameStateHandler.team1Prefab);
            }
            else
            {
                gameStateHandler.players[i] = Instantiate(gameStateHandler.team2Prefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameStateHandler.ball.transform.position = new Vector3(gameStateHandler.ballPosition.x, gameStateHandler.ballPosition.y, 5); ;
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            gameStateHandler.players[i].transform.position = new Vector3(gameStateHandler.playerPositions[i].x, gameStateHandler.playerPositions[i].y, 5);
        }
    }
}
