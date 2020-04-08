using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    GameObject client;
    UDPClient clientData;

    [SerializeField]
    GameObject team1Prefab;
    [SerializeField]
    GameObject team2Prefab;
    [SerializeField]
    GameObject ballPrefab;
    GameObject ball;
    GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CLIENT");
        clientData = client.GetComponent<UDPClient>();

        ball = Instantiate(ballPrefab);
        players = new GameObject[clientData.playerCount];
        for (int i = 0; i < clientData.playerCount; i++)
        {
            if (i < clientData.playerCount / 2)
            {
                players[i] = Instantiate(team1Prefab);
            }
            else
            {
                players[i] = Instantiate(team2Prefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ball.transform.position = clientData.ballPosition;
        for (int i = 0; i < clientData.playerCount; i++)
        {
            players[i].transform.position = clientData.playerPositions[i];
        }
    }
}
