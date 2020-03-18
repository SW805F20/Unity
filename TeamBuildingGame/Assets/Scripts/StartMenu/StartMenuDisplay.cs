using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
public class StartMenuDisplay : MonoBehaviour
{
    GameObject display;
    TMP_InputField inputField;
    GameObject playersConnected;
    TMP_Text connectionText;
    public void Awake()
    {
        display = GameObject.Find("Display");
        inputField = display.GetComponentInChildren<TMP_InputField>();
        playersConnected = GameObject.Find("PlayersConnected");
        connectionText = playersConnected.GetComponentInChildren<TMP_Text>();
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
            inputField.gameObject.SetActive(false);
            connectionText.text = $"Connected to host with IP {inputField.text} \n (1 of 4 players connected)";
            playersConnected.SetActive(true);
        }

    }

    public void CancelConnection()
    {
        // Should cancel the connection to the host
        inputField.gameObject.SetActive(true);
        playersConnected.SetActive(false);
    }
}
