# IPK24 Chat Client

This is a simple chat client application developed in C# for communication with a chat server. It supports both TCP and UDP transport protocols for connection.

## Usage

### Command Line Arguments

The program accepts the following command line arguments:

- `-t <tcp/udp>`: Transport protocol used for connection (mandatory).
- `-s <IP/hostname>`: Server IP address or hostname (mandatory).
- `-p <port>`: Server port (default is 4567).
- `-d <timeout>`: UDP confirmation timeout (default is 250).
- `-r <retransmissions>`: Maximum number of UDP retransmissions (default is 3).
- `-h`: Prints program help output and exits.

Example usage:
```bash
ipk24chat_client.exe -t tcp -s 127.0.0.1 -p 4567 -d 250 -r 3
```

### Local Commands

- `/auth {Username} {Secret} {DisplayName}`: Authenticates the user with the server.
- `/join {ChannelID}`: Joins a specific channel on the server.
- `/rename {DisplayName}`: Locally changes the display name of the user.
- `/help`: Prints out supported local commands with their parameters and a description.

## Classes

### `ServerSetings`

This class handles parsing command line arguments and storing server settings. It also provides methods to print program help and validate server address.

### `TcpUser`

This class implements the functionality for communication over TCP protocol. It handles user authentication, joining channels, sending messages, and receiving messages from the server.

### `UdpUser`

This class implements the functionality for communication over UDP protocol. It handles user authentication, joining channels, sending messages, and receiving messages from the server.

## Program Flow

- Parse command line arguments to get server settings.
- Based on the transport protocol specified, either create a TCP or UDP client instance.
- If TCP, authenticate user and start communication loop.
- If UDP, authenticate user and start communication loop.
- Handle user input and execute corresponding actions (authentication, joining channels, sending messages, etc.).
- Receive messages from the server and display them to the user.
- Gracefully handle program termination by sending BYE message to the server and closing network streams.
```
