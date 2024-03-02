using ipk24chat_client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Classes
{
    class ServerSetings : IServerSettings
    {
        public void PrintHelp()
        {
            Console.WriteLine("Program Help:");
            Console.WriteLine("-t <tcp/udp>          : Transport protocol used for connection");
            Console.WriteLine("-s <IP/hostname>      : Server IP or hostname");
            Console.WriteLine("-p <port>             : Server port");
            Console.WriteLine("-d <timeout>          : UDP confirmation timeout");
            Console.WriteLine("-r <retransmissions>  : Maximum number of UDP retransmissions");
            Console.WriteLine("-h                    : Prints program help output and exits");
        }

        // Default values
        public string transportProtocol { get; } = null;
        public string serverAddress { get; } = null;
        public ushort serverPort { get; } = 4567;
        public ushort udpConfirmationTimeout { get; } = 250;
        public byte maxUdpRetransmissions { get; } = 3;
        public ServerSetings(string[] args)
        {
#if DEBUG
            transportProtocol = "tcp";
            serverAddress = "127.0.0.1";
#endif
            // Parse command line arguments
            for (int i = 0; i < args.Length; i += 2)
            {
                switch (args[i])
                {
                    case "-t":
                        transportProtocol = args[i + 1];
                        break;
                    case "-s":
                        serverAddress = args[i + 1];
                        if (!IPAddress.TryParse(serverAddress, out IPAddress ipAddress))
                        {
                            // It's not a valid IPv4 address, try resolving the domain name
                            try
                            {
                                IPAddress[] addresses = Dns.GetHostAddresses(serverAddress);

                                // Pick the first IPv4 address from the array
                                ipAddress = addresses.FirstOrDefault(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                                serverAddress = ipAddress.ToString();
                                if (ipAddress == null)
                                {
                                    Console.WriteLine("Error: Unable to resolve domain to IPv4 address");
                                    // Handle error when unable to resolve domain to IPv4 address
                                    return;
                                }
                            }
                            catch (System.Net.Sockets.SocketException)
                            {
                                Console.WriteLine("Error: Invalid server address");
                                // Handle invalid server address (neither IPv4 nor valid domain)
                                return;
                            }
                        }
                        break;
                    case "-p":
                        if (ushort.TryParse(args[i + 1], out ushort serverPort)) this.serverPort = serverPort;
                        break;
                    case "-d":
                        if (ushort.TryParse(args[i + 1], out ushort udpConfirmationTimeout)) this.udpConfirmationTimeout = udpConfirmationTimeout;
                        break;
                    case "-r":
                        if (byte.TryParse(args[i + 1], out byte maxUdpRetransmissions)) this.maxUdpRetransmissions = maxUdpRetransmissions;
                        break;
                    case "-h":
                        PrintHelp();
                        return;
                    default:
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        PrintHelp();
                        return;
                }
            }
            // Check for mandatory arguments
            if (string.IsNullOrEmpty(transportProtocol) || string.IsNullOrEmpty(serverAddress))
            {
                Console.WriteLine("Mandatory arguments -t and -s must be specified.");
                PrintHelp();
                return;
            }

            // Perform program logic using the parsed arguments
    #if DEBUG
            Console.WriteLine($"Using {transportProtocol} protocol to connect to server {serverAddress}:{this.serverPort}");
            Console.WriteLine($"UDP Confirmation Timeout: {this.udpConfirmationTimeout}");
            Console.WriteLine($"Maximum UDP Retransmissions: {this.maxUdpRetransmissions}");
    #endif
        }
    }
}
