using System;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace BattleshipsClient
{
    class Client
    {
        Game game = new Game();
        TcpClient tcpclnt;

        public Client()
        {
            tcpclnt = new TcpClient();
            Console.WriteLine("\nConnecting.....");
            tcpclnt.Connect(Message.IP, 8001);
            Console.WriteLine("\nConnected");
        }
        private String receiveMessageString()
        {

            StringBuilder sb = new StringBuilder();
            Stream stm = this.tcpclnt.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
            {
                sb.Append(Convert.ToChar(bb[i]));
            }

            return sb.ToString();
        }

        private Byte[] receiveMessageBytes()
        {

            Stream stm = this.tcpclnt.GetStream();

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            return bb;
        }


        private void receiveCoordinates(out int x, out int y)
        {
            x = BitConverter.ToInt32(receiveMessageBytes(), 0);
            sendMessage(Message.ACK);
            y = BitConverter.ToInt32(receiveMessageBytes(), 0);
            sendMessage(Message.ACK);
        }

        private void sendMessage(String message)
        {

            ASCIIEncoding asen = new ASCIIEncoding();
            Stream stm = this.tcpclnt.GetStream();

            byte[] ba = asen.GetBytes(message);
            Console.WriteLine("\nTransmitting.....");
            stm.Write(ba, 0, ba.Length);


            //TODO wait for ACK
        }
        private void sendMessage(byte[] message)
        {

            Stream stm = this.tcpclnt.GetStream();
            Console.WriteLine("\nTransmitting.....");
            stm.Write(message, 0, message.Length);


            //TODO wait for ACK
        }

        static void Main(string[] args)
        {

            try
            {
                Client c = new Client();

                String serverMessage;

                while ((serverMessage = c.receiveMessageString()) != Message.CYA)
                {
                    Console.Write(serverMessage);

                    int x = -1;
                    int y = -1;

                    switch (serverMessage)
                    {

                        case Message.SET_YOUR_FLEET:
                            Console.WriteLine("\ni am setting my fleet"); //TODO
                            c.game.setTestFleet();
                            c.sendMessage(Message.ACK);
                            break;
                        case Message.WAIT:
                            Console.WriteLine("\ni am waiting");
                            break;
                        case Message.START:
                            c.game.printGameField();
                            break;
                        case Message.SHOOT:
                            Console.WriteLine("\ni am shooting"); //TODO
                            c.game.shoot(out x, out y);
                            c.sendMessage(BitConverter.GetBytes(x));
                            c.sendMessage(BitConverter.GetBytes(y));
                            break;
                        case Message.RECEIVE_COORDINATES:
                            Console.WriteLine("\ni am checking the shooting coordinates"); //TODO
                            x = -1;
                            y = -1;
                            c.receiveCoordinates(out x, out y);
                            Console.WriteLine("\nthe coordinates are: " + x + " " + y); //TODO
                            Boolean newDamage = c.game.checkDamage(x, y);
                            if (newDamage)
                            {
                                Boolean isFinished = c.game.isFinished();
                                if (isFinished)
                                {
                                    c.sendMessage(Message.YOU_HAVE_WON);
                                }
                                else
                                {
                                    c.sendMessage(Message.NEW_DAMAGE);
                                }

                            }
                            else
                            {
                                c.sendMessage(Message.MISSED);
                            }
                            break;
                        case Message.YOU_HAVE_WON:
                            Console.WriteLine("I have won ... great success");
                            break;
                    }
                }
                c.tcpclnt.Close();


                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError..... " + e.StackTrace);

                Console.Read();
            }
        }

    }
}
