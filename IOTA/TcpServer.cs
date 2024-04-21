using IOTA;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IOTA
{
    public class TcpServer
    {
        public static Dictionary<string, Channel> channels;
        public static async Task StartTcpServer(string ipAddress, ushort port, Dictionary<string, Channel> srcChannels)
        {
            channels = srcChannels;
            TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start();
            Console.Error.WriteLine($"TCP server started. Listening on {ipAddress}:{port}...");

            try
            {
                while (true)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    Console.WriteLine("TCP connection accepted from: " + client.Client.RemoteEndPoint);

                    // Handle TCP client communication asynchronously
                    _ = HandleTcpClient(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TCP server error: " + ex.Message);
            }
            finally
            {
                // Stop listening for new clients
                tcpListener.Stop();
            }
        }

        public static async Task HandleTcpClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                    // Get client's IP address and port
                    string clientEndPoint = client.Client.RemoteEndPoint.ToString(); // "IP:Port"

                    // Check if received data matches the expected authentication command format
                    if (dataReceived.StartsWith("AUTH"))
                    {
                        // Log the received message information
                        // Parse the received command
                        string[] parts = dataReceived.Split(' ');
                        if (parts.Length >= 6 && parts[0] == "AUTH" && parts[2] == "AS" && parts[4] == "USING")
                        {
                            string username = parts[1];
                            string displayName = parts[3];
                            string secret = parts[5];
                            Console.WriteLine($"RECV {clientEndPoint} | AUTH Username={username} DisplayName={displayName} Secret={secret}");

                            // Example: Validate the secret for authentication
                            bool isAuthenticated = AuthenticateUser(username, secret);

                            // Prepare response based on authentication result
                            string response = isAuthenticated ?
                                $"REPLY OK IS Auth Success\r\n" :
                                $"REPLY NOK IS Auth UnSuccess\r\n";

                            // Send response back to the client
                            byte[] responseData = Encoding.ASCII.GetBytes(response);
                            await stream.WriteAsync(responseData, 0, responseData.Length);
                            Console.WriteLine($"SENT {clientEndPoint} | REPLY Username={username} DisplayName={displayName} Secret={secret}");
                          
                            
                            // Authenticate user
                            if (isAuthenticated)
                            {
                                // Associate user with default channel (e.g., 'general')
                                string defaultChannelId = "general";
                                if (!channels.ContainsKey(defaultChannelId))
                                    channels[defaultChannelId] = new Channel(defaultChannelId);

                                channels[defaultChannelId].ConnectedUsersTcp.Add(client);
                                //Console.Error.WriteLine($"User {displayName} joined channel {defaultChannelId}");
                                // Find the channel of the sender (assuming displayName is the user's display name)
                                string senderChannelId = null;
                                //DELETE NULL USERS *******************************
                                foreach (var channelId in channels.Keys)
                                {
                                    var channel = channels[channelId];
                                    foreach (var user in channel.ConnectedUsersTcp)
                                    {
                                        if (user.Connected == false)
                                        {
                                            channel.ConnectedUsersTcp.Remove(user);
                                        }
                                    }
                                    if (channel.ConnectedUsersTcp.Any(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint))
                                    {
                                        senderChannelId = channelId;
                                        break;
                                    }
                                }

                                if (senderChannelId != null)
                                {
                                    // Broadcast the message to all users in the sender's channel except the sender
                                    var senderChannel = channels[senderChannelId];
                                    senderChannel.BroadcastMSG($"MSG FROM Server IS {displayName} has joined {senderChannel.ChannelId}\r\n", client.Client.RemoteEndPoint);
                                }
                                else
                                {
                                    //Console.Error.WriteLine($"Sender {displayName} is not connected to any channel.");
                                    // Handle this case based on your application's requirements
                                }


                            }
                            await HandleTcpPacket(client, stream,displayName);

                        }
                        else
                        {
                            // Invalid command format
                            Console.Error.WriteLine("Invalid AUTH command format received.");
                            // Optionally, send an error response back to the client
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Invalid or unexpected data received.");
                        // Optionally, send an error response back to the client
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error handling client: " + ex.Message);
            }
            finally
            {
                // Clean up resources
                client.Close();
            }
        }
        static bool AuthenticateUser(string username, string secret)
        {
            return true;
        }
        public static async Task HandleTcpPacket(TcpClient client, NetworkStream stream,string displayName)
        {
            while (true)
            {
                try
                {

                    byte[] buffer = new byte[1024];
                    int bytesRead2 = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string msgBroadcast = Encoding.ASCII.GetString(buffer, 0, bytesRead2);
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead2).Trim();
                    string clientEndPoint = client.Client.RemoteEndPoint.ToString(); // "IP:Port"

                    // Parse the received command
                    string[] parts = dataReceived.Split(' ');

                    string commandType = parts[0];

                    if (commandType == "JOIN")
                    {
                        // Handle JOIN command
                        if (parts[0] == "JOIN" && parts[2] == "AS")
                        {
                            string channelId = parts[1];
                            displayName = parts[3];

                            //RECIEVE JOIN
                            Console.WriteLine($"RECV {clientEndPoint} | JOIN ChannelID={channelId} DisplayName={displayName}");
                            // Send response back to the client
                            byte[] responseData = Encoding.ASCII.GetBytes("REPLY OK IS Join Success\r\n");
                            await stream.WriteAsync(responseData, 0, responseData.Length);
                            Console.WriteLine($"SENT {clientEndPoint} | REPLY DisplayName={displayName} ChannelID={channelId}");

                            // Check if the user is already connected to any other channels
                            //Remove user connection from current channel
                            //Send Leave MSG
                            foreach (var existingChannelId in channels.Keys.ToList())
                            {
                                if (existingChannelId != channelId) // Exclude the new channel
                                {
                                    var existingChannel = channels[existingChannelId];

                                    // Check if the user is in this existing channel
                                    var userToRemove = existingChannel.ConnectedUsersTcp.FirstOrDefault(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint);
                                    if (userToRemove != null)
                                    {
                                        existingChannel.ConnectedUsersTcp.Remove(userToRemove);
                                        //Console.Error.WriteLine($"User {displayName} removed from channel {existingChannelId}");
                                        // Broadcast the message to all users in the sender's channel except the sender
                                        var senderChannel1 = channels[existingChannelId];
                                        
                                        senderChannel1.BroadcastMSG($"MSG FROM Server IS {displayName} has left {existingChannelId}\r\n", client.Client.RemoteEndPoint);


                                        // Check if the channel is now empty after removing the user
                                        if (existingChannel.ConnectedUsersTcp.Count == 0 && existingChannel.ConnectedUsersUdp.Count== 0)
                                        {
                                            // Channel is empty, remove it from the dictionary
                                            channels.Remove(existingChannelId);
                                            //Console.Error.WriteLine($"Channel {existingChannelId} has become empty and was removed");
                                        }
                                    }
                                }
                            }
                            //Channel exist?
                            if (!channels.ContainsKey(channelId))
                                channels[channelId] = new Channel(channelId);
                            //Add user to channel
                            channels[channelId].ConnectedUsersTcp.Add(client);
                           
                            // For server Stderr
                            //Console.Error.WriteLine($"User {displayName} joined channel {channelId}");

                            //Send Join MSG to channel
                            var senderChannel = channels[channelId];
                            senderChannel.BroadcastMSG($"MSG FROM Server IS {displayName} has joined {channelId}\r\n", client.Client.RemoteEndPoint);
                        }
                        else
                        {
                            Console.Error.WriteLine("Invalid JOIN command format received.");
                            // Optionally, send an error response back to the client
                        }
                    }
                    else if (commandType == "MSG")
                    {
                        // Handle MSG command
                        if (parts[0] == "MSG" && parts[1] == "FROM" && parts[3] == "IS")
                        {
                            displayName = parts[2];
                            string[] messageParts = new ArraySegment<string>(parts, 4, parts.Length - 4).ToArray();
                            string messageContent = string.Join(" ", messageParts);
                            Console.WriteLine($"RECV {clientEndPoint} | MSG DisplayName={displayName} MessageContent={messageContent}");

                            // Find the channel of the sender (assuming displayName is the user's display name)
                            string senderChannelId = null;
                            foreach (var channelId in channels.Keys)
                            {
                                var channel = channels[channelId];
                                if (channel.ConnectedUsersTcp.Any(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint))
                                {
                                    senderChannelId = channelId;
                                    break;
                                }
                            }

                            if (senderChannelId != null)
                            {
                                // Broadcast the message to all users in the sender's channel except the sender
                                var senderChannel = channels[senderChannelId];
                                senderChannel.BroadcastMSG(msgBroadcast, client.Client.RemoteEndPoint);
                            }
                            else
                            {
                                Console.Error.WriteLine($"Sender {displayName} is not connected to any channel.");
                                // Handle this case based on your application's requirements
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("Invalid MSG command format received.");
                            // Optionally, send an error response back to the client
                        }
                    }
                    else if (commandType == "BYE")
                    {
                        // Find the channel of the sender (assuming displayName is the user's display name)
                        string senderChannelId = null;
                        Console.WriteLine($"RECV {clientEndPoint} | BYE DisplayName={displayName}");
                        foreach (var channelId in channels.Keys)
                        {
                            var channel = channels[channelId];
                            if (channel.ConnectedUsersTcp.Any(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint))
                            {
                                senderChannelId = channelId;
                                var userToRemove = channel.ConnectedUsersTcp.FirstOrDefault(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint);
                                if(userToRemove!=null)
                                channel.ConnectedUsersTcp.Remove(userToRemove);
                                if (senderChannelId != null)
                                {
                                    // Broadcast the message to all users in the sender's channel except the sender
                                    var senderChannel = channels[senderChannelId];
                                    senderChannel.BroadcastMSG($"MSG FROM Server IS {displayName} has left {senderChannelId}\r\n", client.Client.RemoteEndPoint);
                                }
                                if (channel.ConnectedUsersTcp.Count == 0 && channel.ConnectedUsersUdp.Count == 0)
                                {
                                    // Channel is empty, remove it from the dictionary
                                    channels.Remove(channelId);
                                    Console.Error.WriteLine($"Channel {channelId} has become empty and was removed");
                                }
                                break;
                            }
                        }
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine("Unsupported command type.");
                        // Optionally, send an error response back to the client
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error handling packet: " + ex.Message);
                    string clientEndPoint = client.Client.RemoteEndPoint.ToString(); // "IP:Port"
                    string senderChannelId = null;
                    foreach (var channelId in channels.Keys)
                    {
                        var channel = channels[channelId];
                        if (channel.ConnectedUsersTcp.Any(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint))
                        {
                            senderChannelId = channelId;
                            var userToRemove = channel.ConnectedUsersTcp.FirstOrDefault(user => user.Client.RemoteEndPoint.ToString() == clientEndPoint);
                            if (userToRemove != null)
                                channel.ConnectedUsersTcp.Remove(userToRemove);
                            if (senderChannelId != null)
                            {
                                // Broadcast the message to all users in the sender's channel except the sender
                                var senderChannel = channels[senderChannelId];
                                senderChannel.BroadcastMSG($"MSG FROM Server IS {displayName} has left {senderChannelId}\r\n", client.Client.RemoteEndPoint);
                            }
                            if (channel.ConnectedUsersTcp.Count == 0 && channel.ConnectedUsersUdp.Count == 0)
                            {
                                // Channel is empty, remove it from the dictionary
                                channels.Remove(channelId);
                                Console.Error.WriteLine($"Channel {channelId} has become empty and was removed");
                            }
                            break;
                        }
                    }
                    return;
                }
            }
        }
    }
}
