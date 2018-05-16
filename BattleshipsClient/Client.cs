using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using BattleshipsServer;

namespace BattleshipsClient
{
    class Client
    {
        public Game game = new Game();
        public TcpClient tcpclnt;

        public Client()
        {
            tcpclnt = new TcpClient();
            Console.WriteLine("\nConnecting.....");
            tcpclnt.Connect(Message.IP, 8001);
            Console.WriteLine("\nConnected");
        }
        public String receiveMessageString()
        {

            StringBuilder sb = new StringBuilder();
            Stream stm = this.tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            byte[] bb = new byte[100]; //TODO change buffer  
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++) //TODO 0 appended?????
            {
                sb.Append(Convert.ToChar(bb[i]));
            }

            return sb.ToString();
        }

        public Byte[] receiveMessageBytes()
        {

            Stream stm = this.tcpclnt.GetStream();

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            return bb;
        }


        public void receiveCoordinates(out int x, out int y)
        {
            x = BitConverter.ToInt32(receiveMessageBytes(), 0);
            sendMessage(Message.ACK);
            y = BitConverter.ToInt32(receiveMessageBytes(), 0);
            sendMessage(Message.ACK);
        }

        public void sendMessage(String message)
        {

            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stm = this.tcpclnt.GetStream();

            byte[] ba = asen.GetBytes(message);
            stm.Write(ba, 0, ba.Length);


            //TODO wait for ACK
        }
        public void sendMessage(byte[] message)
        {

            Stream stm = this.tcpclnt.GetStream();
            stm.Write(message, 0, message.Length);


            //TODO wait for ACK
        }

        
        [STAThread]
        static void Main(string[] args)
            
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.ShowDialog();
        }

     
    }
}
