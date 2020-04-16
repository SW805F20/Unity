using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
namespace sw8
{
    public class TcpClient : MonoBehaviour
    {
        private Vector2[] goalPositions;
        private GoalZoneController goalZoneController;

        public GameObject playingfield;

        // Start is called before the first frame update
        void Start()
        {
            goalPositions = new Vector2[2];
            goalZoneController = playingfield.GetComponent<GoalZoneController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary> Receives the datagram. 
        /// <param name="res">The result associated with the callback - the data.</param>
        private void Receive(IAsyncResult res)
        {

        }

        /// <summary> 
        ///  Gets the datagram message and decodes it to find the type of message.
        ///  Depending on the type it calls the appropiate method to handle the message.
        /// </summary>
        /// <param name="datagramMessage">The hex message send from the game server</param>
        private void DatagramHandler(string datagramMessage)
        {
            // Remove 0x from string before parsing
            datagramMessage = datagramMessage.Remove(0, 2);
            long data;
            byte type;
            if (long.TryParse(datagramMessage, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out data))
            {
                type = (byte)data;
                switch (type)
                {
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
        /// <summary>
        /// Updates the goals positions.
        /// </summary>
        /// <param name="data">The datagram message in hex with the 0x removed</param>
        private void GoalPositionHandler(long data)
        {
            // Get data correctly and save in goalPositions

            goalZoneController.SpawnGoals(goalPositions[0], goalPositions[1]);
        }

    }

}
