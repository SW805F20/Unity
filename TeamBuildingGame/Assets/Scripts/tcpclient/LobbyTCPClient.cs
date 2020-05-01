using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine.SceneManagement;

public class LobbyTCPClient : MonoBehaviour
{
    GameStateHandler gameStateHandler;
    private TcpClient tcpClient;
    ConnectionHandler connectionHandler;
    bool connected = false;

    void Awake()
    {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        connectionHandler = GameObject.FindGameObjectWithTag("ConnectionHandler").GetComponent<ConnectionHandler>();
    }

    public void CreateConnection(){
        try
        {
            // Establishes a tcp connection on the port.
            tcpClient = new TcpClient(connectionHandler.tcpIPAddr, connectionHandler.tcpPort);

            if (tcpClient.Connected) {
                Console.WriteLine("Connected to: {0}:{1}", connectionHandler.tcpIPAddr, connectionHandler.tcpPort);
                connected = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(connected && tcpClient.GetStream().DataAvailable)
        {
            StartListening();
        }
    }

    private void StartListening()
    {
        NetworkStream stream = tcpClient.GetStream();

        // Is 2 because the first 2 bytes of a message contains the length of the rest of the package
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
        Debug.Log("LOADING SCENE!");
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
        Debug.Log("SCENE LOADED!");
    }

    private void PlayerTagHandler(long data)
    {
        ushort tag_id = (ushort)(data >> 16);
        byte player_id = (byte)(data >> 24);

    }

    private void AnchorPositionHandler(long data)
    {
        byte id = (byte)(data >> 8);
        ushort x = (ushort)(data >> 16);
        ushort y = (ushort)(data >> 32);
        // increment by 1 because the ID starts from 0
        switch(id + 1)
        {
            case 1:
                gameStateHandler.anchor1.x = x;
                gameStateHandler.anchor1.y = y;
                break;
            case 2:
                gameStateHandler.anchor2.x = x;
                gameStateHandler.anchor2.y = y;
                break;
            case 3:
                gameStateHandler.anchor3.x = x;
                gameStateHandler.anchor3.y = y;
                break;
            case 4:
                gameStateHandler.anchor4.x = x;
                gameStateHandler.anchor4.y = y;
                // We have received all anchors and can render the playing field
                // gameStateHandler.fieldGeneratorScript.CreatePlayingField();
                break;
        }
    }

    /// <summary>
    /// Updates the goals positions.
    /// </summary>
    /// <param name="data">The datagram message in hex with the 0x removed</param>
    private void GoalPositionHandler(long data)
    {
        byte teamId = (byte)(data >> 8);
        ushort x = (ushort)(data >> 16);
        ushort y = (ushort)(data >> 32);
        byte goalZoneCenterOffset = (byte)(data >> 48);

        // gameStateHandler.goalZoneControllerScript.SpawnGoal(new Vector2(x, y), goalZoneCenterOffset, teamId);
    }

}
