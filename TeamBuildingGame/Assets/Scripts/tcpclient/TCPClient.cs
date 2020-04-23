﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class TCPClient : MonoBehaviour
{
    private TcpClient tcpClient;
    private string ipAddress = "192.168.0.89";
    private int portNumber = 10000;
    private Vector2[] goalPositions;
    private GoalZoneController goalZoneControllerScript;
    private FieldGenerator fieldGeneratorScript;

    [SerializeField]
    private GameObject GoalZoneController;

    [SerializeField]
    private GameObject PlayingField;



    void Awake()
    {
        try
        {
            // Establishes a tcp connection on the port.
            tcpClient = new TcpClient(ipAddress, portNumber);

            if (tcpClient.Connected)
                Console.WriteLine("Connected to: {0}:{1}", ipAddress, portNumber);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        goalPositions = new Vector2[2];
        goalZoneControllerScript = GoalZoneController.GetComponent<GoalZoneController>();
        fieldGeneratorScript = PlayingField.GetComponent<FieldGenerator>();

        StartListening();
    }

    // Update is called once per frame
    void Update()
    {
        if(tcpClient.GetStream().DataAvailable)
        {
            StartListening();
        }
    }

    void OnDestroy()
    {
        // Ensures connection is closed when the game object it is attached to is destroyed, so it wont continue to receive messages after the game ends.
        tcpClient.Close();
    }


    private void StartListening()
    {
        NetworkStream stream = tcpClient.GetStream();


        byte[] data = new byte[2];

        int bytes = stream.Read(data, 0, data.Length);
        string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        int package_length = 0;
        try
        {
            package_length = Int32.Parse(responseData);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{responseData}'");
        }

        byte[] package = new byte[package_length];

        bytes = stream.Read(package, 0, package.Length);
        responseData = System.Text.Encoding.ASCII.GetString(package, 0, bytes);

        DatagramHandler(responseData);

    }

    /// <summary> 
    ///  Gets the datagram message and decodes it to find the type of message.
    ///  Depending on the type it calls the appropiate method to handle the message.
    /// </summary>
    /// <param name="datagramMessage">The hex message sent from the game server</param>
    private void DatagramHandler(string datagramMessage)
    {
        // Remove 0x from string before parsing
        if (datagramMessage.ToLower().StartsWith("0x"))
        {
            datagramMessage = datagramMessage.Remove(0, 2);
        }

        long data;
        byte type;
        if (long.TryParse(datagramMessage, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out data))
        {
            type = (byte)data;
            switch (type)
            {
                case 1:
                    AnchorPositionHandler(data);
                    break;
                case 2:
                    PlayerTagHandler(data);
                    break;
                case 3:
                    HandleGameStart(data);
                    break;
                case 5:
                    GoalPositionHandler(data);
                    break;
            }
        }
        else
        {
            Debug.LogError("Network data could not be parsed");
        }
        

    }

    private void HandleGameStart(long data)
    {
        throw new NotImplementedException();
    }

    private void PlayerTagHandler(long data)
    {
        byte player_id = (byte)(data >> 8);
        ushort tag_id = (ushort)(data >> 24);

    }

    private void AnchorPositionHandler(long data)
    {
        byte id = (byte)(data >> 8);
        ushort x = (ushort)(data >> 16);
        ushort y = (ushort)(data >> 32);
        switch(id + 1)
        {
            case 1:
                fieldGeneratorScript.anchor1.x = x;
                fieldGeneratorScript.anchor1.y = y;
                break;
            case 2:
                fieldGeneratorScript.anchor2.x = x;
                fieldGeneratorScript.anchor2.y = y;
                break;
            case 3:
                fieldGeneratorScript.anchor3.x = x;
                fieldGeneratorScript.anchor3.y = y;
                break;
            case 4:
                fieldGeneratorScript.anchor4.x = x;
                fieldGeneratorScript.anchor4.y = y;
                // We have received all anchors and can render the playing field
                fieldGeneratorScript.CreatePlayingField();
                break;
        }
    }

    /// <summary>
    /// Updates the goals positions.
    /// </summary>
    /// <param name="data">The datagram message in hex with the 0x removed</param>
    private void GoalPositionHandler(long data)
    {
        // Get data correctly and save in goalPositions
        goalZoneControllerScript.SpawnGoals(goalPositions[0], goalPositions[1]);
    }

}

