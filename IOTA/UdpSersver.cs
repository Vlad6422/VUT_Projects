using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IOTA
{
    public class UdpServer
    {
        public static async Task StartUdpServer(ushort port, ushort timeout, byte maxRetransmissions)
        {
            UdpClient udpListener = new UdpClient(port);
            Console.WriteLine($"UDP server started. Listening on  {port}...");

            try
            {
                while (true)
                {
                    UdpReceiveResult result = await udpListener.ReceiveAsync();
                    Console.WriteLine("UDP packet received from: " + result.RemoteEndPoint);

                    // Handle UDP packet
                    HandleUdpPacket(result.Buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UDP server error: " + ex.Message);
            }
        }

        static void HandleUdpPacket(byte[] data)
        {
            string message = Encoding.ASCII.GetString(data);
            Console.WriteLine("UDP packet received: " + message);
        }

        // You can add more methods related to UDP server handling if needed
    }
}
