using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void GoalScoredEvent();
public class TCPClient : MonoBehaviour
{
    [SerializeField]
    GameObject gameState;
    [SerializeField]
    Text startMenuText;

    GameStateHandler gameStateHandler;
    private TcpClient tcpClient;
    ConnectionHandler connectionHandler;
    bool connected = false;

    public event GoalScoredEvent OnGoalScored;


    void Awake()
    {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        connectionHandler = GameObject.Find("ConnectionHandler").GetComponent<ConnectionHandler>();
    }

    public bool CreateConnection()
    {
        try
        {
            // Establishes a tcp connection on the port.
            tcpClient = new TcpClient(connectionHandler.tcpIPAddr, connectionHandler.tcpPort);

            if (tcpClient.Connected)
            {
                connected = true;
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(connected && tcpClient.GetStream().DataAvailable)
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
                    HandleGameStart();
                    break;
                case 4:
                    GoalScoredHandler(data);
                    break;
                case 5:
                    GoalPositionHandler(data);
                    break;
                case 6:
                    PlayerAndGoalAmountHandler(data);
                    break;
            }
        }
        else
        {
            Debug.LogError("Network data could not be parsed");
        }
    }
    private void GoalScoredHandler(long data)
    {
        byte team1Score = (byte)(data >> 8);
        byte team2Score = (byte)(data >> 16);
        gameStateHandler.team1Score = team1Score;
        gameStateHandler.team2Score = team2Score;
        OnGoalScoredHandler();
    }
    private void OnGoalScoredHandler()
    {
        OnGoalScored?.Invoke();
    }

    private void HandleGameStart()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    private void PlayerTagHandler(long data)
    {
        string tag_id = "0x" + Convert.ToString((ushort)(data >> 8), 16);
        byte player_id = (byte)(data >> 24);

        gameStateHandler.myPlayerId = player_id;
        gameStateHandler.myPlayerTag = tag_id;

        startMenuText.text += $"\n\n Your tag is {gameStateHandler.myPlayerTag}";

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

        gameStateHandler.goalCenterOffset = goalZoneCenterOffset;
        if(teamId == 0)
        {
            gameStateHandler.blueGoal = new Vector2(x, y);
        }
        else if(teamId == 1)
        {
            gameStateHandler.redGoal = new Vector2(x, y);
        }
        if(gameStateHandler.goalZoneControllerScript)
        {
            gameStateHandler.goalZoneControllerScript.SpawnGoals();
        }
    }

    /// <summary>
    /// Defines the amount of players and goals needed to win.
    /// </summary>
    /// <param name="data">The datagram message in hex with the 0x removed</param>
    private void PlayerAndGoalAmountHandler(long data)
    {
        byte goalAmount = (byte)(data >> 8);
        byte playerAmount = (byte)(data >> 16);
        gameStateHandler.playerCount = playerAmount;
        gameStateHandler.goalsToWin = goalAmount;
    }

}
