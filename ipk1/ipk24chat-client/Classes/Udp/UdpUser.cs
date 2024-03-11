using ipk24chat_client.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace ipk24chat_client.Classes.Udp
{
    public class UdpUser
    {
        private string _username { get; set; }
        private string _secret { get; set; }
        private string _displayName { get; set; }
        private string _message { get; set; }
        private ushort _messageId { get; set; }
        private UdpClient _client;
        private IPEndPoint _serverEndPoint;
        public UdpUser(string IpAdress, ushort port)
        {
            _username = string.Empty;
            _secret = string.Empty;
            _displayName = string.Empty;
            _message = string.Empty;
            _messageId = 0;
            _client = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(IpAdress), port);
            // client.Connect(serverEndPoint);
        }
        public void WriteInternalError(string error)
        {
            Console.Error.WriteLine($"ERR: {error}");
        }
        public bool ChangeUserName(string username)
        {
            if (username.Length > 20 || !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[A-Za-z0-9\-]+$"))
            {
                WriteInternalError("Too Big UserName or Incorect");
                return false;
            }
            else
            {
                _username = username;

            }
            return true;

        }
        public bool ChangeSecret(string secret)
        {
            if (secret.Length > 128 || !System.Text.RegularExpressions.Regex.IsMatch(secret, @"^[A-Za-z0-9\-]+$"))
            {
                WriteInternalError("Too Big Secret or Incorect");
                return false;
            }
            else
            {
                _secret = secret;

            }
            return true;

        }
        public bool ChangeDisplayName(string newName)
        {
            if (_displayName.Length > 20)
            {
                WriteInternalError("Too Big DisplayName or Incorect");
                return false;
            }
            else
            {
                _displayName = newName;
            }
            return true;

        }
        public void Start()
        {
            Thread receiveThread = new Thread(RecieveUdpPacket);
            receiveThread.Start();
            while (true)
            {
                string? userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    WriteInternalError("Empty input. Please enter a command or message.");
                    continue;
                }


                if (userInput.StartsWith("/"))
                {
                    // Handle commands
                    string[] commandParts = userInput.Substring(1).Split(' ');

                    if (commandParts.Length == 0)
                    {
                        WriteInternalError("Invalid command.Please provide a valid command.");
                        continue;
                    }

                    string commandName = commandParts[0].ToLower();

                    switch (commandName)
                    {
                        case "auth":
                            if (commandParts.Length != 4)
                            {
                                WriteInternalError("Invalid number of parameters for /auth command.");
                                continue;
                            }
                            if (
                            ChangeUserName(commandParts[1]) &&
                            ChangeDisplayName(commandParts[2]) &&
                            ChangeSecret(commandParts[3]))
                            {
                                Authenticate();
                            }

                            break;

                        case "join":
                            if (commandParts.Length != 2)
                            {
                                WriteInternalError("Invalid number of parameters for /join command.");
                                continue;
                            }
                              JoinChannel(commandParts[1]);
                            break;

                        case "rename":
                            if (commandParts.Length != 2)
                            {
                                WriteInternalError("Invalid number of parameters for /rename command.");
                                continue;
                            }
                            ChangeDisplayName(commandParts[1]);
                            break;

                        case "help":
                            // Print out supported local commands with their parameters and a description
                            Console.WriteLine("/auth {Username} {Secret} {DisplayName} - Sends AUTH message to the server");
                            Console.WriteLine("/join {ChannelID} - Sends JOIN message to the server");
                            Console.WriteLine("/rename {DisplayName} - Locally changes the display name of the user");
                            Console.WriteLine("/help - Prints out supported local commands with their parameters and a description");
                            break;

                        default:
                            WriteInternalError($"Unknown command '{commandName}'. Type '/help' for a list of supported commands.");
                            break;
                    }
                }
                else
                {
                    // Handle sending messages to the server
                    // This part should be implemented based on your application logic
                    _message = userInput;
                    if (_message == "BYE")
                    {
                        ByeMessage byeMessage = new ByeMessage();
                        byte[] byeMessageBytes = { 0xFF, 0, 99 };
                        _client.Send(byeMessageBytes, byeMessageBytes.Length, _serverEndPoint);
                        _client.Close();
                        return;
                    }
                    else
                    {
                        //  SendMessage("MSG FROM " + _displayName + " IS " + _message + "\r\n");
                        //  Console.WriteLine(RecieveMessage());
                    }
#if DEBUG
                    Console.WriteLine($"Sending message to the server: {userInput}");
#endif           
                    //break;
                }
            }

        }
        void Authenticate()
        {
            AuthMessage authMessage = new AuthMessage(_messageId, _username, _displayName, _secret);
            _client.Send(authMessage.GET(), authMessage.GET().Length, _serverEndPoint);


            Thread receiveThread = new Thread(RecieveUdpPacket);
            receiveThread.Start();
            _messageId++;

        }
        public void JoinChannel(string channelName)
        {
            if (channelName.Length > 20 || !System.Text.RegularExpressions.Regex.IsMatch(channelName, @"^[A-Za-z0-9\-]+$"))
            {
                WriteInternalError("Too Big ChannelName OR Incorrect");
            }
            else
            {
                JoinMessage joinMessage = new JoinMessage(_messageId, channelName, _displayName);
                _client.Send(joinMessage.GET(),joinMessage.GET().Length, _serverEndPoint);
            }

            // Console.WriteLine(RecieveMessage());
        }
        void RecieveUdpPacket()
        {
            _serverEndPoint = new IPEndPoint(_serverEndPoint.Address, 0);
            while (true)
            {
                try
                {
                    byte[] buff = _client.Receive(ref _serverEndPoint);
                    ushort result = BitConverter.ToUInt16(buff, 1);
                    ConfirmMessage confirmMessage = new ConfirmMessage(result);
                    if (buff[0] != 0x00)
                    {
                        _client.Send(confirmMessage.GET(), confirmMessage.GET().Length, _serverEndPoint);
                        if (buff[0] == 0x01)
                        {
                            ReplyMessage replyMessage = new ReplyMessage(buff);
                            if (replyMessage.Result == 1)
                            {
                                Console.WriteLine("Success: " + replyMessage.MessageContents);
                            }
                            else if (replyMessage.Result == 0)
                            {
                                Console.WriteLine("Failure: " + replyMessage.MessageContents);
                            }
                        }

                        if (buff[0] == 0x04)
                        {
                            MsgMessage msgMessage = new MsgMessage(buff);
                            Console.WriteLine(msgMessage.DisplayName + ": " + msgMessage.MessageContents);
                        }
                        if (buff[0] == 0xFE)
                        {
                            ErrMessage errMessage = new ErrMessage(buff);
                            Console.WriteLine("ERR FROM " + errMessage.DisplayName + ": " + errMessage.MessageContents);
                        }
                        if (buff[0] == 0xFF)
                        {
                            _client.Close();
                            return;
                        }
                    }

                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
