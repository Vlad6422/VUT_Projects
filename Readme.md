# IPK24 Chat Server🔮

This is a simple chat server application developed in C# to facilitate communication with chat clients. The server supports both TCP and UDP transport protocols for client connections.

## Download and Setup💻

1. **Clone the repository:** Clone this repository to your local machine using the following command:

    ```bash
    git clone https://git.fit.vutbr.cz/xmalas04/ipk-project2.git
    ```

2. **Navigate to the project directory:** Change your current directory to the cloned project folder:

    ```bash
    cd ipk-project2
    ```

3. **Build the project:** Build the application using the .NET CLI:

    ```bash
    make
    ```
4. **Run the server** Run the server:
    ```bash
    ./ipk24chat-server -l 0.0.0.0 -p 4567 -d 250 -r 3
    ```

## Command Line Arguments🎓

The server program accepts the following command line arguments:

- `-l <IP>`: IP address to listen on for incoming connections (mandatory).
- `-p <port>`: Port number to listen on for incoming connections (mandatory).
- `-d <timeout>`: UDP confirmation timeout (default is 250).
- `-r <retransmissions>`: Maximum number of UDP retransmissions (default is 3).
- `-h`: Prints program help output and exits.

Example usage:
```bash
./ipk24chat-server -l 0.0.0.0 -p 4567 -d 250 -r 3
```

## Program Behavior📘

### Client Communication

- **User Authentication (`AUTH`):** The server handles initial user authentication upon client connection. Once authenticated, the server automatically joins the user to a default channel.
  
- **Channel Join (`JOIN`):** Users can join specific channels after successful authentication. The server creates the channel if it doesn't exist and broadcasts a message to other users in the channel upon successful join.

- **Sending Messages (`MSG`):** Authenticated users can send messages to channels they have joined. The server forwards these messages to other users in the same channel, excluding the sender.

- **Error Handling (`ERR`):** The server gracefully handles any errors received from clients, terminating the corresponding user session if needed.

- **Session Termination (`BYE`):** Upon receiving a `BYE` message, the server cleans up resources associated with the user's session and removes them from any joined channels.

### Logging

The server logs incoming and outgoing messages in the following format to standard output (stdout):

```
RECV {FROM_IP}:{FROM_PORT} | {MESSAGE_TYPE}[MESSAGE_CONTENTS]
SENT {TO_IP}:{TO_PORT} | {MESSAGE_TYPE}[MESSAGE_CONTENTS]
```

Where `{MESSAGE_TYPE}` corresponds to message type keywords (`AUTH`, `JOIN`, `MSG`, `ERR`, `BYE`), and `{MESSAGE_CONTENTS}` includes relevant message details (e.g., `Username=user1 DisplayName=User1 Secret=xXx`).

## Testing the IPK24 Chat Server Application🔎

### Automated Testing⌛

Automated testing ensures the reliability and functionality of the IPK24 Chat Server across various scenarios. Input files with predefined commands and expected server responses are used to validate application behavior.

#### Testing Scenarios:

1. **Basic Functionality Test:** Verifies fundamental server features such as user authentication, channel joining, message sending, and error handling.

2. **Error Handling Test:** Validates the server's ability to handle erroneous commands and unexpected client responses effectively.

3. **Performance Test:** Tests the server's responsiveness and scalability under different load conditions.

#### Testing Input File:

An example testing input file (`testing_input.txt`) contains predefined commands and expected server responses, allowing comprehensive testing of application functionality.

### Manual Verification with Wireshark

Manual verification using Wireshark captures and analyzes network traffic between the server and clients, ensuring compliance with TCP and UDP protocols and validating message integrity.

## Conclusion📣

Thorough testing of the IPK24 Chat Server ensures reliability, stability, and performance. Automated testing and manual verification with Wireshark enhance confidence in the server's functionality, ensuring a seamless user experience.