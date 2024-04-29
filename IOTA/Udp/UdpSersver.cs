using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace IOTA.Udp
{
    public class UdpServer
    {
        public static Dictionary<string, Channel> channels;
        public static ushort Timeout;
        public static byte MaxRetransmissions;
        public static async Task StartUdpServer(string ipAdr,ushort port, ushort timeout, byte maxRetransmissions, Dictionary<string, Channel> srcChannels)
        {
            channels = srcChannels;
            Timeout = timeout;
            MaxRetransmissions = maxRetransmissions;
            IPAddress localIpAddress = IPAddress.Parse(ipAdr);
            IPEndPoint localEndPoint = new IPEndPoint(localIpAddress,port);
            UdpClient udpListener = new UdpClient(localEndPoint);
            Console.Error.WriteLine($"UDP server started. Listening on {localEndPoint}...");

            try
            {
                while (true)
                {
                    UdpReceiveResult result = await udpListener.ReceiveAsync();
                    writeRecvPacket(result.Buffer, result.RemoteEndPoint.ToString());
                    ushort messageID = BitConverter.ToUInt16(result.Buffer, 1);
                    await udpListener.SendAsync(new ConfirmMessage(messageID).GET(), result.RemoteEndPoint);
                    Console.WriteLine($"SENT {result.RemoteEndPoint} | CONFIRM");
                    // Handle UDP packet and send confirmation
                    _ = HandleUdpPacketAuthAsync(udpListener, result);

                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("UDP server error: " + ex.Message);
            }
        }

        static async Task HandleUdpPacketAuthAsync(UdpClient udpClient, UdpReceiveResult receiveResult)
        {
            UdpClient UdpClient = new UdpClient();
            UdpClient.Connect(receiveResult.RemoteEndPoint);

            try
            {
                byte[] data = receiveResult.Buffer;
                //AUTH
                if (data[0] == 0x02)
                { // Decode received message
                    AuthMessage authMessage = new AuthMessage(data);
                    Random random = new Random();
                    ReplyMessage replyMessage = new ReplyMessage((ushort)random.Next(0, ushort.MaxValue + 1), 0x01, authMessage.MessageID, "AUTH Succ ");

                    // Send reply packet back to the client
                    //WAIT FOR CONFITM
                    bool confirmationReceived = await SendAndWaitForConfirmationAsync(UdpClient, replyMessage.MessageID, "REPLY", replyMessage.GetBytes(), MaxRetransmissions + 1, Timeout);


                    string defaultChannelId = "general";
                    if (!channels.ContainsKey(defaultChannelId))
                        channels[defaultChannelId] = new Channel(defaultChannelId);

                    channels[defaultChannelId].ConnectedUsersUdp.Add(UdpClient);

                    string? senderChannelId = null;

                    foreach (var channelId in channels.Keys)
                    {
                        var channel = channels[channelId];
                        if (channel.ConnectedUsersUdp.Any(user => user.Client.RemoteEndPoint.ToString() == UdpClient.Client.RemoteEndPoint.ToString()))
                        {
                            senderChannelId = channelId;
                            break;
                        }
                    }

                    if (senderChannelId != null)
                    {
                        // Broadcast the message to all users in the sender's channel except the sender
                        var senderChannel = channels[senderChannelId];
                        senderChannel.BroadcastMSG($"MSG FROM Server IS {authMessage.DisplayName} has joined {senderChannel.ChannelId}\r\n", UdpClient.Client.RemoteEndPoint);
                    }

                    else
                    {
                        Console.Error.WriteLine($"Sender {authMessage.DisplayName} is not connected to any channel.");
                        // Handle this case based on your application's requirements
                    }
                    await HandleUdpPacketMsgJoinAsync(UdpClient, authMessage.DisplayName);
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error handling UDP packet: " + ex.Message);
            }
        }
        static async Task HandleUdpPacketMsgJoinAsync(UdpClient udpClient, string DisplayName)
        {
            string lastDisplayName = DisplayName;
            while (true)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    writeRecvPacket(result.Buffer, udpClient.Client.RemoteEndPoint.ToString());
                    ushort messageID = BitConverter.ToUInt16(result.Buffer, 1);
                    if (result.Buffer[0] != 0x00)
                    {
                        await udpClient.SendAsync(new ConfirmMessage(messageID).GET());
                        Console.WriteLine($"SENT {udpClient.Client.RemoteEndPoint} | CONFIRM");
                    }
                    switch (result.Buffer[0])
                    {
                        //JOIN
                        case 0x03:
                            {
                                JoinMessage joinMessage = new JoinMessage(result.Buffer);
                                ReplyMessage replyMessage = new ReplyMessage(10, 0x01, joinMessage.MessageID, "JOIN Succ ");

                                // Send reply packet back to the client
                                bool confirmationReceived = await SendAndWaitForConfirmationAsync(udpClient, replyMessage.MessageID, "REPLY", replyMessage.GetBytes(), MaxRetransmissions + 1, Timeout);

                                // Check if the user is already connected to any other channels
                                //Remove user connection from current channel
                                //Send Leave MSG
                                foreach (var existingChannelId in channels.Keys.ToList())
                                {
                                    if (existingChannelId != joinMessage.ChannelID) // Exclude the new channel
                                    {
                                        var existingChannel = channels[existingChannelId];

                                        // Check if the user is in this existing channel
                                        var userToRemove = existingChannel.ConnectedUsersUdp.FirstOrDefault(user => user.Client.RemoteEndPoint.ToString() == udpClient.Client.RemoteEndPoint.ToString());
                                        if (userToRemove != null)
                                        {
                                            existingChannel.ConnectedUsersUdp.Remove(udpClient);
                                            Console.Error.WriteLine($"User {joinMessage.DisplayName} removed from channel {existingChannelId}");
                                            // Broadcast the message to all users in the sender's channel except the sender
                                            var senderChannel1 = channels[existingChannelId];

                                            senderChannel1.BroadcastMSG($"MSG FROM Server IS {joinMessage.DisplayName} has left {existingChannelId}\r\n", udpClient.Client.RemoteEndPoint);


                                            // Check if the channel is now empty after removing the user
                                            if (existingChannel.ConnectedUsersTcp.Count == 0 && existingChannel.ConnectedUsersUdp.Count == 0)
                                            {
                                                // Channel is empty, remove it from the dictionary
                                                channels.Remove(existingChannelId);
                                                Console.Error.WriteLine($"Channel {existingChannelId} has become empty and was removed");
                                            }
                                        }
                                    }
                                }
                                //Channel exist?
                                if (!channels.ContainsKey(joinMessage.ChannelID))
                                    channels[joinMessage.ChannelID] = new Channel(joinMessage.ChannelID);
                                //Add user to channel
                                channels[joinMessage.ChannelID].ConnectedUsersUdp.Add(udpClient);

                                // For server Stderr
                                Console.Error.WriteLine($"User {joinMessage.DisplayName} joined channel {joinMessage.ChannelID}");

                                //Send Join MSG to channel
                                var senderChannel = channels[joinMessage.ChannelID];
                                senderChannel.BroadcastMSG($"MSG FROM Server IS {joinMessage.DisplayName} has joined {joinMessage.ChannelID}\r\n", udpClient.Client.RemoteEndPoint);
                                break;
                            }
                        //MSG
                        case 0x04:
                            {
                                MsgMessage msgMessage = new MsgMessage(result.Buffer);
                                string senderChannelId = null;
                                lastDisplayName = msgMessage.DisplayName;
                                foreach (var channelId in channels.Keys)
                                {
                                    var channel = channels[channelId];
                                    if (channel.ConnectedUsersUdp.Any(user => user.Client.RemoteEndPoint.ToString() == udpClient.Client.RemoteEndPoint.ToString()))
                                    {
                                        senderChannelId = channelId;
                                        break;
                                    }
                                }

                                if (senderChannelId != null)
                                {
                                    // Broadcast the message to all users in the sender's channel except the sender
                                    var senderChannel = channels[senderChannelId];
                                    senderChannel.BroadcastMSG($"MSG FROM {msgMessage.DisplayName} IS {msgMessage.MessageContents}\r\n", udpClient.Client.RemoteEndPoint);
                                }
                                else
                                {
                                    Console.Error.WriteLine($"Sender {msgMessage.DisplayName} is not connected to any channel.");
                                    // Handle this case based on your application's requirements
                                }
                                break;

                            }
                        //BYE
                        case 0xFF:
                            // Find the channel of the sender (assuming displayName is the user's display name)
                            string senderChannelID = null;
                            foreach (var channelId in channels.Keys)
                            {
                                var channel = channels[channelId];
                                if (channel.ConnectedUsersUdp.Any(user => user.Client.RemoteEndPoint.ToString() == udpClient.Client.RemoteEndPoint.ToString()))
                                {
                                    senderChannelID = channelId;
                                    var userToRemove = channel.ConnectedUsersUdp.FirstOrDefault(user => user.Client.RemoteEndPoint.ToString() == udpClient.Client.RemoteEndPoint.ToString());
                                    if (userToRemove != null)
                                        channel.ConnectedUsersUdp.Remove(userToRemove);
                                    if (senderChannelID != null)
                                    {
                                        // Broadcast the message to all users in the sender's channel except the sender
                                        var senderChannel = channels[senderChannelID];
                                        senderChannel.BroadcastMSG($"MSG FROM Server IS {lastDisplayName} has left {senderChannelID}\r\n", udpClient.Client.RemoteEndPoint);
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
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
        }
        public static void writeRecvPacket(byte[] data, string endPoint)
        {
            switch (data[0])
            {
                case 0x00:
                    Console.WriteLine($"RECV {endPoint} | CONFIRM");
                    break;
                case 0x01:
                    Console.WriteLine($"RECV {endPoint} | REPLY");
                    break;
                case 0x02:
                    Console.WriteLine($"RECV {endPoint} | AUTH");
                    break;
                case 0x03:
                    Console.WriteLine($"RECV {endPoint} | JOIN");
                    break;
                case 0x04:
                    Console.WriteLine($"RECV {endPoint} | MSG");
                    break;
                case 0xFE:
                    Console.WriteLine($"RECV {endPoint} | ERR");
                    break;
                case 0xFF:
                    Console.WriteLine($"RECV {endPoint} | BYE");
                    break;
            }
        }
        public static async Task<bool> SendAndWaitForConfirmationAsync(UdpClient udpClient, ushort messageID, string typeofPacket, byte[] messageToSend, int maxResendAttempts = 3, int timeoutMilliseconds = 250)
        {
            bool confirmationReceived = false;
            int resendCount = 0;

            while (!confirmationReceived && resendCount < maxResendAttempts)
            {
                // Send the message
                await udpClient.SendAsync(messageToSend, messageToSend.Length);
                Console.WriteLine($"SENT {udpClient.Client.RemoteEndPoint} | {typeofPacket}");

                DateTime startTime = DateTime.UtcNow;

                while (!confirmationReceived && (DateTime.UtcNow - startTime).TotalMilliseconds < timeoutMilliseconds)
                {
                    var receiveTask = udpClient.ReceiveAsync();
                    var delayTask = Task.Delay(10); // Poll every 10ms

                    await Task.WhenAny(receiveTask, delayTask);

                    if (receiveTask.IsCompleted)
                    {
                        byte[] receivedData = receiveTask.Result.Buffer;

                        // Check confirmation criteria
                        if (receiveTask.IsCompleted && receiveTask.Result.Buffer.Length >= 3 &&
                                receiveTask.Result.Buffer[0] == 0x00 &&
                                BitConverter.ToUInt16(receiveTask.Result.Buffer, 1) == messageID)
                        {
                            // Confirmation packet received
                            confirmationReceived = true;
                            writeRecvPacket(receivedData, udpClient.Client.RemoteEndPoint.ToString());
                        }
                    }
                }

                resendCount++;
            }

            return confirmationReceived;
        }
    }
}

