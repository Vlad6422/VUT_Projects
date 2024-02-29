using ipk24chat_client.Classes;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
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
                    // Create a TcpClient
                    using (TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork))
                    {
                        tcpClient.Connect(serverSetings.serverAddress, serverSetings.serverPort);
                        
                        Console.WriteLine("Connected to the server.");
                        // Get the network stream
                        using (NetworkStream networkStream = tcpClient.GetStream())
                        {
                            // Prepare the message to be sent
                            string message = "AUTH xmalas04 AS Vlad USING 5b78c74a-4b20-49e8-a709-e0146dadd9c6\r\n";
                            // Convert the message to bytes
                            byte[] data = Encoding.ASCII.GetBytes(message);
                            // Send the data to the server
                            networkStream.Write(data, 0, data.Length);
                            Console.WriteLine($"Sent: {message}");


                            // Optional: Receive a response from the server
                            byte[] buffer = new byte[1024];
                            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine($"Received from server: {response}");

                            message = "MSG FROM Vlad IS TetsText\r\n";
                            // Convert the message to bytes
                            data = Encoding.ASCII.GetBytes(message);
                            // Send the data to the server
                            networkStream.Write(data, 0, data.Length);
                            Console.WriteLine($"Sent: {message}");

                            bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                            response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine($"Received from server: {response}");

                            message = "BYE\r\n";
                            data = Encoding.ASCII.GetBytes(message);
                            networkStream.Write(data, 0, data.Length);
                            Console.WriteLine($"Sent: {message}");

                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else if (serverSetings.transportProtocol == "udp")
            {
                Console.WriteLine("Work In Progress");
            }
            
            Console.ReadLine();
        }


    }
}


