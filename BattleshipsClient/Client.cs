using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using BattleshipsServer;

namespace BattleshipsClient
{
    class Client
    {
        private TcpClient tcpclnt;

        internal Client()
        {
            //TODO uncomment for actual playing
            tcpclnt = new TcpClient();
            Console.WriteLine("\nConnecting.....");
            tcpclnt.Connect(Message.IP, 8001);
            Console.WriteLine("\nConnected");
        }

        internal String ReceiveMessageString()
        {
            StringBuilder sb = new StringBuilder();
            Stream stm = this.tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            byte[] buffer = new byte[10];
            int k = stm.Read(buffer, 0, buffer.Length);

            for (int i = 0; i < k; i++)
            {
                sb.Append(Convert.ToChar(buffer[i]));
            }

            return sb.ToString();
        }

        internal Byte[] ReceiveMessageBytes()
        {
            Stream stm = this.tcpclnt.GetStream();

            byte[] buffer = new byte[100];
            int k = stm.Read(buffer, 0, 100);

            return buffer;
        }


        internal void ReceiveCoordinates(out int x, out int y)
        {
            x = BitConverter.ToInt32(ReceiveMessageBytes(), 0);
            SendMessage(Message.ACK);
            y = BitConverter.ToInt32(ReceiveMessageBytes(), 0);
            SendMessage(Message.ACK);
        }

        internal void SendMessage(String message)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stm = this.tcpclnt.GetStream();

            byte[] ba = asen.GetBytes(message);
            stm.Write(ba, 0, ba.Length);
        }

        internal void SendMessage(byte[] message)
        {
            Stream stm = this.tcpclnt.GetStream();
            stm.Write(message, 0, message.Length);
        }

        internal void CloseConnection()
        {
            tcpclnt.Close();
            return;
        }
     
    }
}
