﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace BattleshipsServer
{
    class Server
    {

        private Socket p1;
        private Socket p2;
        private byte[] buffer = new byte[10];
        private bool isFinished = false;

        private Server()
        {
            
            IPAddress ipAd = IPAddress.Parse(Message.IP);
            TcpListener myListener = new TcpListener(ipAd, 8001);
            myListener.Start();
            Console.WriteLine("The server is running at port 8001...");
            Console.WriteLine("The local End point is  :" + myListener.LocalEndpoint);
            Console.WriteLine("Waiting for player 1 .....");
            p1 = myListener.AcceptSocket();
            Console.WriteLine("Connection accepted from " + p1.RemoteEndPoint);
            Console.WriteLine("Waiting for player 2 .....");
            sendMessage(p1, Message.WAIT_FOR_PLAYER_2);
            p2 = myListener.AcceptSocket();
            Console.WriteLine("Connection accepted from " + p2.RemoteEndPoint);

            Console.WriteLine("Stop Listening to Connections");
            myListener.Stop();

            playGame();

            Console.WriteLine("Closing connection to + " + p1.RemoteEndPoint);
            p1.Close();
            Console.WriteLine("Closing connection to + " + p2.RemoteEndPoint);
            p2.Close();
        }
        
        static void Main(string[] args)
        {
            Server myServer = new Server();
        }

        private void turn(Socket p1, Socket p2)
        {
            sendMessage(p1, Message.SHOOT); //send shoot instruction to p1
            int x = -1;
            int y = -1;
            receiveCoordinates(p1, out x, out y); //receive the chosen coordinates form p1
            sendMessage(p2, Message.RECEIVE_COORDINATES); //inform p2 that he has to check damage-coordinates
            sendCoordinates(p2, x, y); // send coordinates to p2
            sendMessage(p2, Message.CHECK_DAMAGE);
            String msg = receiveMessage(p2); // receive damageMessage
            sendMessage(p1, msg);  //send the damageMessage to p1
            if (msg == Message.YOU_HAVE_WON) //TODO changed from .equals to ==
            {
                isFinished = true;
                Console.WriteLine("I am exiting now");
            }
        }

        private void playGame()
        {
            bool p1Turn = true;

            sendMessageToAll(Message.SET_YOUR_FLEET);
            receiveMessage(p1);
            receiveMessage(p2);

            sendMessageToAll(Message.START); //TODO changed from "Start" to Message.Start
            while (!isFinished)
            {
                if (p1Turn)
                {
                    turn(p1, p2);
                    p1Turn = false;
                }
                else
                {
                    turn(p2, p1);
                    p1Turn = true;
                }
            }

            sendMessageToAll(Message.CYA);
        }

        private void sendCoordinates(Socket s, int x, int y)
        {
            sendMessage(s, BitConverter.GetBytes(x));
            s.Receive(buffer); //Wait for ACK
            Console.WriteLine(s.RemoteEndPoint + " has sent: " + convertToStringMessage(buffer));
            sendMessage(s, BitConverter.GetBytes(y));
            Array.Clear(buffer, 0, buffer.Length);
            s.Receive(buffer);
            Console.WriteLine(s.RemoteEndPoint + " has sent: " + convertToStringMessage(buffer));
        }

        private void sendMessage(Socket s, byte[] data)
        {
            Console.WriteLine("\nsending " + convertToStringMessage(data) + " to " + s.RemoteEndPoint);
            try
            {
                s.Send(data);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void sendMessage(Socket s, String data)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] toSend = asen.GetBytes(data);
            sendMessage(s, toSend);
        }
        
        private void sendMessageToAll(String data)
        {
            sendMessage(p1, data);
            sendMessage(p2, data);
        }

        private void receiveCoordinates(Socket s, out int x, out int y) 
        {
            //TODO socketException after youWin
            s.Receive(buffer);
            x = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Received x: " + x);
            Array.Clear(buffer, 0, buffer.Length);
            s.Receive(buffer);
            y = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Received y: " + y);
        }

        private String receiveMessage(Socket s)
        {
            s.Receive(buffer);
            String messageReceived = convertToStringMessage(buffer);
            Console.WriteLine("\nReceived " + messageReceived + " form " + s.RemoteEndPoint);

            return messageReceived;
        }

        private String convertToStringMessage(byte[] b)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] != 0) // dont append the 0 from the byte[]
                {
                    sb.Append(Convert.ToChar(b[i]));
                }
            }
            return sb.ToString();
        }

    }
}
