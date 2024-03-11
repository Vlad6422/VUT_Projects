using ipk24chat_client.Classes;
using ipk24chat_client.Classes.Tcp;
using ipk24chat_client.Classes.Udp;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace ipk24chat_client
{

    internal class Program
    {
        static void Main(string[] args)
        {

            ServerSetings serverSetings = new ServerSetings(args);

#if DEBUG
            Console.WriteLine(serverSetings.transportProtocol + "  " + serverSetings.serverAddress + ":" + serverSetings.serverPort + "   " + serverSetings.maxUdpRetransmissions + "    " + serverSetings.udpConfirmationTimeout);
#endif
            if (serverSetings.transportProtocol == "tcp")
            {
                try
                {
                    using (TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork))
                    {
                        tcpClient.Connect(serverSetings.serverAddress, serverSetings.serverPort);
                        //Console.WriteLine("Connected to the server.");
                        //START
                        using (NetworkStream networkStream = tcpClient.GetStream())
                        {
                            TcpUser tcpUser = new TcpUser(networkStream);
                            tcpUser.Start();
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Err: {ex.Message}");
                }
            }
            else if (serverSetings.transportProtocol == "udp")
            {
                UdpUser udpUser = new UdpUser(serverSetings.serverAddress,serverSetings.serverPort);
                udpUser.Start();
                

            }

        }

    }


}



