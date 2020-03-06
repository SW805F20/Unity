using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class UDPClient : MonoBehaviour
{
    UdpClient uClient;
    Socket uSocket;

    void Awake()
    {
        uClient = new UdpClient(10000);

        uSocket = uClient.Client;

        
        uSocket.SetSocketOption(SocketOptionLevel.Socket,
                      SocketOptionName.Broadcast, 1);


        uClient.JoinMulticastGroup(IPAddress.Parse("224.3.29.71"));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartListening();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        uClient.Close();
    }

    public void StartListening()
    {
        try
        {
            uClient.BeginReceive(new AsyncCallback(recv), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        try
        {

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = uClient.EndReceive(res, ref RemoteIpEndPoint);
            uClient.BeginReceive(new AsyncCallback(recv), null);

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

        StartListening();
    }
}
