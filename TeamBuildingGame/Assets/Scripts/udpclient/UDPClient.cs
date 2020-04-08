using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;

/// <summary> UDPClient to be instantiated on each player. Starts listening for datagrams from a remote host.</summary>
public class UDPClient : MonoBehaviour
{
    UdpClient uClient;
    string ipAddress = "224.3.29.71";
    int portNumber = 10000;
    string datagramMessage;
    string datagramSender;

    public int playerCount;
    public Vector2[] playerPositions;
    public Vector2 ballPosition;

    void Awake()
    {
        playerPositions = new Vector2[playerCount];
        ballPosition = new Vector2();

        // Establishes a udp connection on the port.
        uClient = new UdpClient(portNumber);

        // Adds the client to a multicastgroup based on the given ip address
        uClient.JoinMulticastGroup(IPAddress.Parse(ipAddress));
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
        string returnData = System.Text.Encoding.ASCII.GetString(receiveBytes);
        datagramMessage = returnData;
        datagramSender = "Adress: " + RemoteIpEndPoint.Address.ToString() + ", port: " + RemoteIpEndPoint.Port.ToString();

        // Saves position of players in array of vector 2d
        GetNetworkDataType(datagramMessage);
    }
    private void GetNetworkDataType(string datagramMessage)
    {
        datagramMessage = datagramMessage.Remove(0, 2);
        long data;
        byte type;
        if (long.TryParse(datagramMessage, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out data))
        {
            type = (byte)data;
            switch (type)
            {
                case 1:
                    SavePlayerData(data);
                    break;
                default:
                    Debug.LogError("Data format incorrect: Type not recognized");
                    break;
            }
        }
        else
        {
            Debug.LogError("Network data could not be parsed");
        }

    }
    private void SavePlayerData(long data)
    {
        byte id = (byte)(data >> 16);
        ushort x = (ushort)(data >> 24);
        ushort y = (ushort)(data >> 40);

        if(id == 0)
        {
            ballPosition.x = x;
            ballPosition.y = y;
        }
        else
        {
            id--; // Use id to enter correct index of player array which is 0 indexed. The lowest player id is 1.
            playerPositions[id].x = x;
            playerPositions[id].y = y;
        }
    }
}
