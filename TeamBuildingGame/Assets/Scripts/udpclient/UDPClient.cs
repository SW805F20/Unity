using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class UDPClient : MonoBehaviour
{
    UdpClient uClient;
    string ipAddress = "224.3.29.71";
    int portNumber = 10000;

    void Awake()
    {
        //Establiehs a udp connection on the port.
        uClient = new UdpClient(portNumber);

        //Adds the client to a multicastgroup based on the given ip address
        uClient.JoinMulticastGroup(IPAddress.Parse(ipAddress));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartListening();
    }

    void OnDestroy()
    {
        //Ensures connection is closed when the game object it is attached to is destroyed, so it wont continue to receive messages after the game ends.
        uClient.Close();
    }

    /// <summary> Starts receiving from the remote host. Uses an asynchronous callback delegate the invokes the recv function.
    ///    the null is an object representing the state.</summary>
    public void StartListening()
    {
        try
        {
            uClient.BeginReceive(new AsyncCallback(Receive), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary> Receives the datagram. 
    /// <param name="res">The result associated with the callback - the data.</param>
    public void Receive(IAsyncResult res)
    {
        //Represents a network endpoint as IP address and port number.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        try
        {
            // Receives the message as an array of bytes, then ends communication with the remote endpoint.
            Byte[] receiveBytes = uClient.EndReceive(res, ref RemoteIpEndPoint);

            //Restarts communication again to receive a new datagram.
            uClient.BeginReceive(new AsyncCallback(Receive), null);

            //The bytes that were received are converted to a string, which is written to the unity debug log.
            string returnData = System.Text.Encoding.ASCII.GetString(receiveBytes);

            Debug.Log("This is the message you received " +
                                      returnData.ToString());
            Debug.Log("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
