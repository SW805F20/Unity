using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    GameObject client;
    UDPClient clientData;

    [SerializeField]
    GameObject playerPrefab;
    GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        client = GameObject.Find("CLIENT");
        clientData = client.GetComponent<UDPClient>();

        players = new GameObject[clientData.playerCount];
        for (int i = 0; i < clientData.playerCount; i++)
        {
            players[i] = Instantiate(playerPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < clientData.playerCount; i++)
        {
            players[i].transform.position = clientData.playerPositions[i];
        }
    }
}
