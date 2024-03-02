using ipk24chat_client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Classes
{
    class TcpUser : IUser,IStream
    {
        public string Username { get; set; } = null;
        public string Secret { get; set; } = null;
        public string DisplayName { get; set; } = null;
        public string ChannelId { get; set; } = null;
        public string Message { get; set; } = null;
        public NetworkStream networkStream { get; }
        public TcpUser(NetworkStream networkStream) {
            this.networkStream = networkStream;
        }
        public void Start()
        {
            while (true)
            {
                string userInput = Console.ReadLine();

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
                    }
                    else {
                        SendMessage("MSG FROM " + DisplayName + " IS " + Message + "\r\n");
                        Console.WriteLine(RecieveMessage());
                    }
                    Console.WriteLine($"Sending message to the server: {userInput}");
                    
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
            Console.WriteLine(RecieveMessage());
        }
        public void JoinChannel(string channelName)
        {
            SendMessage("JOIN "+channelName+" AS "+DisplayName+"\r\n");
            Console.WriteLine(RecieveMessage());
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
            byte[] buffer = new byte[1024];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return response;

        }
        public void ChangeDisplayName(string newName)
        {
            DisplayName = newName;
        }
    }
}
