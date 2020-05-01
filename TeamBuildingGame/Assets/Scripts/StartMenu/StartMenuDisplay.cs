using System.Collections;
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

    [SerializeField]
    private GameObject connectionHandlerObject;
    ConnectionHandler connectionHandler;

    public void Awake(){
        connectionHandler = connectionHandlerObject.GetComponent<ConnectionHandler>();
    }

    public void Start()
    {
        playersConnected.SetActive(false);

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
            connectionText.text = $"Connected to host with IP {connectionHandler.tcpIPAddr} \n (1 of 4 players connected)";
            playersConnected.SetActive(true);

            connectionHandler.GetComponent<LobbyTCPClient>().CreateConnection();
        }
    }

    public void CancelConnection()
    {
        // Should cancel the connection to the host
        inputField.gameObject.SetActive(true);
        playersConnected.SetActive(false);
    }
}
