using ipk24chat_client.Interfaces;
using System.Net.Sockets;
using System.Text;


namespace ipk24chat_client.Classes
{
     public class TcpUser : IUser,IStream
    {
        public string Username { get; set; }
        public string Secret { get; set; }
        public string DisplayName { get; set; }
        public string ChannelId { get; set; }
        public string Message { get; set; }
        public NetworkStream networkStream { get; }
        public TcpUser(NetworkStream networkStream) {
            this.networkStream = networkStream;
            Username = string.Empty;
            Secret = string.Empty;
            DisplayName = string.Empty;
            ChannelId = string.Empty;
            Message = string.Empty;
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
                    Console.WriteLine("Error: Empty input. Please enter a command or message.");
                    continue;
                }
             

                if (userInput.StartsWith("/"))
                {
                    // Handle commands
                    string[] commandParts = userInput.Substring(1).Split(' ');

                    if (commandParts.Length == 0)
                    {
                        Console.WriteLine("Error: Invalid command. Please provide a valid command.");
                        continue;
                    }

                    string commandName = commandParts[0].ToLower();

                    switch (commandName)
                    {
                        case "auth":
                            if (commandParts.Length != 4)
                            {
                                Console.WriteLine("Error: Invalid number of parameters for /auth command.");
                                continue;
                            }

                            Username = commandParts[1];
                            DisplayName = commandParts[2];
                            Secret = commandParts[3];

                            Authenticate();

                            break;

                        case "join":
                            if (commandParts.Length != 2)
                            {
                                Console.WriteLine("Error: Invalid number of parameters for /join command.");
                                continue;
                            }
                            // Handle /join command
                            // Send JOIN message to the server with channel name
                            JoinChannel(commandParts[1]);
                            break;

                        case "rename":
                            if (commandParts.Length != 2)
                            {
                                Console.WriteLine("Error: Invalid number of parameters for /rename command.");
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
                            Console.WriteLine($"Error: Unknown command '{commandName}'. Type '/help' for a list of supported commands.");
                            break;
                    }
                }
                else
                {
                    // Handle sending messages to the server
                    // This part should be implemented based on your application logic
                    Message = userInput;
                    if(Message == "BYE")
                    {
                        SendMessage(Message+"\r\n");
                        return;
                    }
                    else {
                        SendMessage("MSG FROM " + DisplayName + " IS " + Message + "\r\n");
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
            SendMessage("AUTH " + Username + " AS " + DisplayName + " USING " + Secret + "\r\n");
            //Console.WriteLine(RecieveMessage());
        }
        public void JoinChannel(string channelName)
        {
            SendMessage("JOIN "+channelName+" AS "+DisplayName+"\r\n");
           // Console.WriteLine(RecieveMessage());
        }
        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            // Send the data to the server
            networkStream.Write(data, 0, data.Length);

        }
        public string RecieveMessage()
        {
            // Optional: Receive a response from the server

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                return response;
            }
            catch(IOException)
            {
                return "ERROR";
            }
               

        }
        public void StartReceivingMessages()
        {
            while (Message!="BYE")
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
                            if(resultType == "OK")
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
                
                // Добавьте вашу логику обработки полученных сообщений здесь
            }
        }
        public void ChangeDisplayName(string newName)
        {
            DisplayName = newName;
        }
    }
}
