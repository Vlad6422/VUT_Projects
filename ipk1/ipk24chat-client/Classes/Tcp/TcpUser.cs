using ipk24chat_client.Interfaces;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ipk24chat_client.Classes.Tcp
{
    public class TcpUser : IUser
    {
        private string _username { get; set; }
        private string _secret { get; set; }
        private string _displayName { get; set; }
        private string _message { get; set; }
        private NetworkStream _networkStream { get; }
        public TcpUser(NetworkStream networkStream)
        {
            _networkStream = networkStream;
            _username = string.Empty;
            _secret = string.Empty;
            _displayName = string.Empty;
            _message = string.Empty;
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
            Thread receiveThread = new Thread(StartReceivingMessages);
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
                        SendMessage(_message + "\r\n");
                        return;
                    }
                    else
                    {
                        SendMessage("MSG FROM " + _displayName + " IS " + _message + "\r\n");
                        //  Console.WriteLine(RecieveMessage());
                    }
#if DEBUG
                    Console.WriteLine($"Sending message to the server: {userInput}");
#endif           
                    //break;
                }
            }

        }
        public void Stop()
        {

        }
        public void Authenticate()
        {
            SendMessage("AUTH " + _username + " AS " + _displayName + " USING " + _secret + "\r\n");
            //Console.WriteLine(RecieveMessage());
        }
        public void JoinChannel(string channelName)
        {
            if (channelName.Length > 20 || !System.Text.RegularExpressions.Regex.IsMatch(channelName, @"^[A-Za-z0-9\-]+$"))
            {
                WriteInternalError("Too Big ChannelName OR Incorrect");
            }
            else
            {
                SendMessage("JOIN " + channelName + " AS " + _displayName + "\r\n");
            }

            // Console.WriteLine(RecieveMessage());
        }
        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            // Send the data to the server
            _networkStream.Write(data, 0, data.Length);

        }
        public string RecieveMessage()
        {

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = _networkStream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                return response;
            }
            catch (IOException)
            {
                return "ERROR";
            }


        }
        public void StartReceivingMessages()
        {
            while (_message != "BYE")
            {
                string response = RecieveMessage();
                if (response != "BYE")
                {
                    string[] parts = response.Split();
                    string msgType = parts[0];

                    switch (msgType)
                    {
                        case "MSG":
                            string displayName = parts[2];
                            string messageContent = string.Join(" ", parts[4..]);
                            Console.WriteLine($"{displayName}: {messageContent}");
                            break;

                        case "ERR":
                            string errorDisplayName = parts[2];
                            string errorContent = string.Join(" ", parts[4..]);
                            Console.Error.WriteLine($"ERR FROM {errorDisplayName}: {errorContent}");
                            break;

                        case "REPLY":
                            string resultType = parts[1];
                            string MessageContent = string.Join(" ", parts[3..]);
                            if (resultType == "OK")
                            {
                                Console.Error.WriteLine($"Success: {MessageContent}");
                            }
                            else if (resultType == "NOK")
                            {
                                Console.Error.WriteLine($"Failure: {MessageContent}");
                            }

                            break;

                        default:
                            //Console.Error.WriteLine($"Unknown message type: {msgType}");
                            break;
                    }
                    //Console.WriteLine($"Received message from the server: {response}");
                }
                else
                {
                    break;
                }

            }
        }
    }
}
