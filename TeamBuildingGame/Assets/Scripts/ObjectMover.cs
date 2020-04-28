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

    void Awake()
    {
        gameStateHandler = gameState.GetComponent<GameStateHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CLIENT");
        clientData = client.GetComponent<UDPClient>();

        gameStateHandler.ball = Instantiate(gameStateHandler.ballPrefab);
        gameStateHandler.ball.transform.SetParent(gameStateHandler.playingField.transform);

        gameStateHandler.players = new GameObject[gameStateHandler.playerCount];
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            if (i < gameStateHandler.playerCount / 2)
            {
                gameStateHandler.players[i] = Instantiate(gameStateHandler.team1Prefab);
                gameStateHandler.players[i].transform.SetParent(gameStateHandler.playingField.transform);
            }
            else
            {
                gameStateHandler.players[i] = Instantiate(gameStateHandler.team2Prefab);
                gameStateHandler.players[i].transform.SetParent(gameStateHandler.playingField.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBall();
        var fieldoffset = new Vector3(gameStateHandler.middleX, gameStateHandler.middleY, 0);
        for (int i = 0; i < gameStateHandler.playerCount; i++)
        {
            var relativePosition = new Vector3(gameStateHandler.playerPositions[i].x, gameStateHandler.playerPositions[i].y, 0);
            Vector3 newPosition = (gameStateHandler.mainCamera.transform.position + gameStateHandler.mainCamera.transform.forward * gameStateHandler.distanceFromCamera) - fieldoffset + relativePosition;
            gameStateHandler.players[i].transform.position = newPosition;
            gameStateHandler.players[i].transform.rotation = gameStateHandler.mainCamera.transform.localRotation;
        }
    }

    void UpdateBall(){
        var fieldoffset = new Vector3(gameStateHandler.middleX, gameStateHandler.middleY, 0);
        //gameStateHandler.ball.transform.position = new Vector3(gameStateHandler.ballPosition.x, gameStateHandler.ballPosition.y, 5);
        var relativePosition = new Vector3(gameStateHandler.ballPosition.x, gameStateHandler.ballPosition.y, 0);
        Vector3 newPosition = (gameStateHandler.mainCamera.transform.position + gameStateHandler.mainCamera.transform.forward * gameStateHandler.distanceFromCamera) - fieldoffset + relativePosition;
        gameStateHandler.ball.transform.position = newPosition;
        gameStateHandler.ball.transform.rotation = gameStateHandler.mainCamera.transform.localRotation;
    }
}
