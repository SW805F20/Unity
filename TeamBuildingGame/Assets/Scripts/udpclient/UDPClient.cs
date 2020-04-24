using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

/// <summary> UDPClient to be instantiated on each player. Starts listening for datagrams from a remote host.</summary>
public class UDPClient : MonoBehaviour
{
    [SerializeField]
    GameObject gameState;
    GameStateHandler gameStateHandler;
    private UdpClient uClient;
    private string ipAddress = "224.3.29.71";
    private int portNumber = 10000;
    private string datagramMessage;
    private string datagramSender;
    private byte playerPacketTimestamp;
    

    void Awake()
    {
        // Get the game state handler for global variables.
        gameStateHandler = gameState.GetComponent<GameStateHandler>(); 

        gameStateHandler.playerPositions = new Vector2[gameStateHandler.playerCount];
        gameStateHandler.ballPosition = new Vector2();

        // Establishes a udp connection on the port.
        uClient = new UdpClient(portNumber);

        // Adds the client to a multicastgroup based on the given ip address
        uClient.JoinMulticastGroup(IPAddress.Parse(ipAddress));

        playerPacketTimestamp = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartListening();
    }

    void OnDestroy()
    {
        // Ensures connection is closed when the game object it is attached to is destroyed, so it wont continue to receive messages after the game ends.
        uClient.Close();
    }

    /// <summary> Starts receiving from the remote host. Uses an asynchronous callback delegate the invokes the recv function.
    ///    Null is an object representing the state. The state is a user-defined object containing information about the receive operation.
    ///    We do not need to pass any special information, which is why we pass null.</summary>
    private void StartListening()
    {
        uClient.BeginReceive(new AsyncCallback(Receive), null);
    }

    /// <summary> Receives the datagram. 
    /// <param name="res">The result associated with the callback - the data.</param>
    private void Receive(IAsyncResult res)
    {
        // Represents a network endpoint as IP address and port number.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, portNumber);

        // Receives the message as an array of bytes, then ends communication with the remote endpoint.
        Byte[] receiveBytes = uClient.EndReceive(res, ref RemoteIpEndPoint);

        // Restarts communication again to receive a new datagram.
        uClient.BeginReceive(new AsyncCallback(Receive), null);

        // The bytes that were received are converted to a string, which is written to the unity debug log.
        string returnData = Encoding.ASCII.GetString(receiveBytes);
        datagramMessage = returnData;
        datagramSender = "Adress: " + RemoteIpEndPoint.Address.ToString() + ", port: " + RemoteIpEndPoint.Port.ToString();

        // Saves position of players in array of vector 2d
        DatagramHandler(datagramMessage);
    }

    /// <summary> 
    ///  Gets the datagram message and decodes it to find the type of message.
    ///  Depending on the type it calls the appropiate method to handle the message.
    /// </summary>
    /// <param name="datagramMessage">The hex message sent from the game server</param>
    private void DatagramHandler(string datagramMessage)
    {
        // Remove 0x from string before parsing
        if(datagramMessage.ToLower().StartsWith("0x"))
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
                    UpdatePlayerData(data);
                    break;

            }
        }
        else
        {
            Debug.LogError("Network data could not be parsed");
        }
    }

    /// <summary>
    /// Updates the player and ball position.
    /// Also calls subfunction that checks the timestamp to make sure old data does not get used.
    /// </summary>
    /// <param name="data">The datagram message in hex with the 0x removed</param>
    private void UpdatePlayerData(long data)
    {
        // bitshifting the hex string and typecasting to byte to get the values.
        // see network format in the report for more detail
        byte time = (byte)(data >> 8);
        byte id = (byte)(data >> 16);
        ushort x = (ushort)(data >> 24);
        ushort y = (ushort)(data >> 40);


        if (CheckTimestamp(time))
        {
            if (id == 0)
            {
                gameStateHandler.ballPosition.x = x;
                gameStateHandler.ballPosition.y = y;
            }
            else
            {
                // Player id starts at 1 while the playerposition array is 0 indexed. Decrementing id so that they line up.
                id--;
                gameStateHandler.playerPositions[id].x = x;
                gameStateHandler.playerPositions[id].y = y;
            }
        }
        
    }

    /// <summary>
    /// Uses the timestamp from the incoming message to check against the last timestamp from a playerpacket to make sure old data is not being used.
    /// </summary>
    /// <param name="time">the timestamp from the incoming message</param>
    /// <returns></returns>
    private bool CheckTimestamp(byte time)
    {
        bool result;
        // Should be reworked, breaks if the timestamps from 235 to 255 fails
        if (time < playerPacketTimestamp)
        {
            result = false;
        }
        else if( time <= 20 
                 && playerPacketTimestamp >= 235)
        {
            result = true;
        }
        else
        {
            result = true;
        }

        playerPacketTimestamp = time;
        return result;
    }
}
