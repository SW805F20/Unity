using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class TCPClient : MonoBehaviour
{
    private TcpClient tcpClient;
    private string ipAddress = "127.0.0.1";
    private int portNumber = 10000;
    private Vector2[] goalPositions;
    private GoalZoneController goalZoneControllerScript;
    private FieldGenerator fieldGeneratorScript;

    public GameObject GoalZoneController;
    public GameObject PlayingField;



    void Awake()
    {
        // Establishes a udp connection on the port.
        tcpClient = new TcpClient(ipAddress, portNumber);
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

    }

    void OnDestroy()
    {
        // Ensures connection is closed when the game object it is attached to is destroyed, so it wont continue to receive messages after the game ends.
        tcpClient.Close();
    }

    /// <summary> Receives the datagram. 
    /// <param name="res">The result associated with the callback - the data.</param>
    private void Receive(IAsyncResult res)
    {

    }

    private void StartListening()
    {
        if (!tcpClient.Connected)
        {
            tcpClient.Connect(ipAddress, portNumber);
        }
        Debug.Log("Connected");
        
        NetworkStream stream = tcpClient.GetStream();

        byte[] data = new byte[256];

        int bytes = stream.Read(data, 0, data.Length);
        string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        Debug.Log($"Received: {responseData}");
        DatagramHandler(responseData);

        string message = "Received the anchor positions";
        data = System.Text.Encoding.ASCII.GetBytes(message);

        stream.Write(data, 0, data.Length);

        Console.WriteLine("Sent: {0}", message);

        // Close everything.
        stream.Close();
        tcpClient.Close();
    }

    /// <summary> 
    ///  Gets the datagram message and decodes it to find the type of message.
    ///  Depending on the type it calls the appropiate method to handle the message.
    /// </summary>
    /// <param name="datagramMessage">The hex message send from the game server</param>
    private void DatagramHandler(string datagramMessage)
    {
        // Remove 0x from string before parsing
        string[] hexStrings = datagramMessage.ToLower().Split(new string[] { "0x" }, StringSplitOptions.None);
        long data;
        byte type;

        for (int i = 1; i < hexStrings.Length; i++)
        {
            if (long.TryParse(hexStrings[i], System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out data))
            {
                type = (byte)data;
                switch (type)
                {
                    case 0:
                        AnchorPositionHandler(data, i);
                        break;
                    case 2:
                        GoalPositionHandler(data);
                        break;
                }
            }
            else
            {
                Debug.LogError("Network data could not be parsed");
            }
        }

    }

    private void AnchorPositionHandler(long data, int anchorNumber)
    {
        byte id = (byte)(data >> 8);
        ushort x = (ushort)(data >> 16);
        ushort y = (ushort)(data >> 32);
        switch(anchorNumber)
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

