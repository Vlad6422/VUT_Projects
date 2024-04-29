using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IOTA.Tcp;
using IOTA.Udp;

namespace IOTA
{
    class Program
    {
        public static Dictionary<string, Channel> channels = new Dictionary<string, Channel>();
        static async Task Main(string[] args)
        {
            // Default values
            string ipAddress = "0.0.0.0";
            ushort port = 4567;
            ushort udpTimeout = 250;
            byte maxRetransmissions = 3;

            // Parse command-line arguments
            for (int i = 0; i < args.Length; i += 2)
            {
                switch (args[i])
                {
                    case "-l":
                        ipAddress = args[i + 1];
                        break;
                    case "-p":
                        if (!ushort.TryParse(args[i + 1], out port))
                        {
                            Console.Error.WriteLine("Invalid port number.");
                            return;
                        }
                        break;
                    case "-d":
                        if (!ushort.TryParse(args[i + 1], out udpTimeout))
                        {
                            Console.Error.WriteLine("Invalid UDP timeout value.");
                            return;
                        }
                        break;
                    case "-r":
                        if (!byte.TryParse(args[i + 1], out maxRetransmissions))
                        {
                            Console.Error.WriteLine("Invalid max retransmissions value.");
                            return;
                        }
                        break;
                    case "-h":
                        Console.WriteLine("Usage:");
                        Console.WriteLine("-l <ip_address>   : Server listening IP address for welcome sockets");
                        Console.WriteLine("-p <port>         : Server listening port for welcome sockets");
                        Console.WriteLine("-d <timeout>      : UDP confirmation timeout");
                        Console.WriteLine("-r <retransmits>  : Maximum number of UDP retransmissions");
                        Console.WriteLine("-h                : Print program help output and exit");
                        return;
                    default:
                        Console.Error.WriteLine($"Unknown argument: {args[i]}");
                        return;
                }
            }


             
            Task tcpTask = TcpServer.StartTcpServer(ipAddress, port,channels);
            Task udpTask = UdpServer.StartUdpServer(ipAddress,port, udpTimeout, maxRetransmissions,channels);

            Console.Error.WriteLine("Press any key to exit...");
            Console.ReadKey();

            // Clean up tasks
            await Task.WhenAll(tcpTask, udpTask);
        }

    }
}