﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;


public class StartMenuDisplay : MonoBehaviour
{
    public Text connectionText;
    public InputField inputField;
    public GameObject playersConnected;
    public GameObject display;

    GameStateHandler gameStateHandler;

    [SerializeField]
    private GameObject connectionHandlerObject;
    ConnectionHandler connectionHandler;

    public bool isConnected = false;

    public void Awake(){
        connectionHandlerObject = GameObject.Find("ConnectionHandler");
        connectionHandler = connectionHandlerObject.GetComponent<ConnectionHandler>();
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        if (connectionHandler.tcpIPAddr.Length > 1)
        {
            isConnected = true;
        }
    }

    public void Start()
    {
        playersConnected.SetActive(false);

    }

    public void Update()
    {
        if(isConnected)
        {
            ReturningFromGame();
        }
    }
    public void ConnectToHost()
    {

        if(inputField == null)
        {
            Debug.Log("Could not find component");
        }

        // The provided IP in the input field should be used to connect to the host
        // After succesful connection we check if all players are connected so the game can begin.
        // Otherwise we display the number of players that are connected as a waiting screen.

        if(!String.IsNullOrEmpty(inputField.text))
        {
            connectionHandler.tcpIPAddr = inputField.text;
            inputField.gameObject.SetActive(false);
            bool result = connectionHandler.GetComponent<TCPClient>().CreateConnection();

            if(result)
            {
                connectionText.text = $"Connected to host with IP {connectionHandler.tcpIPAddr} \n (Waiting for other players to connect)";
            }
            else
            {
                connectionText.text = $"No game is being hosted on IP: {connectionHandler.tcpIPAddr}";
            }
            playersConnected.SetActive(true);


        }
    }

    public void CancelConnection()
    {
        // Should cancel the connection to the host
        inputField.gameObject.SetActive(true);
        playersConnected.SetActive(false);
    }

    public void ReturningFromGame() {
        inputField.gameObject.SetActive(false);
        connectionText.text = $"Connected to host with IP {connectionHandler.tcpIPAddr} \n (Waiting for other players to connect)";
        connectionText.text += $"\n\n Your tag is {gameStateHandler.myPlayerTag}";
        playersConnected.SetActive(true);
        isConnected = false;

    } 
}
