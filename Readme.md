# IPK24 Chat Client🔮

This is a simple chat client application developed in C# for communication with a chat server. It supports both TCP and UDP transport protocols for connection.

## Download and Setup💻

1. **Clone the repository:** Clone this repository to your local machine using the following command:

    ```bash
    git clone https://git.fit.vutbr.cz/xmalas04/ipk-project1.git
    ```

2. **Navigate to the project directory:** Change your current directory to the cloned project folder:

    ```bash
    cd ipk-project1
    ```

3. **Build the project:** Build the application using the .NET CLI:

    ```bash
    make
    ```
4. **Run the project** Run the Project:
    ```bash
    ipk24chat_client -t tcp -s 127.0.0.1
    ```
## Usage🎓

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
./ipk24chat-client -t tcp -s 127.0.0.1 -p 4567 -d 250 -r 3
```

### Local Commands

- `/auth {Username} {Secret} {DisplayName}`: Authenticates the user with the server.
- `/join {ChannelID}`: Joins a specific channel on the server.
- `/rename {DisplayName}`: Locally changes the display name of the user.
- `/help`: Prints out supported local commands with their parameters and a description.

## Classes📘

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


## Testing the IPK24 Chat Client Application🔎

In software development, thorough testing is essential to ensure the reliability and functionality of an application. This article discusses the testing process for the IPK24 Chat Client application, including automated testing using various input files and manual verification using Wireshark.

#### Automated Testing⌛

Automated testing plays a crucial role in validating the functionality of the IPK24 Chat Client across different scenarios. The application was tested using a variety of input files, representing different user interactions and server responses. These input files were fed into the application's standard input (stdin) during automated testing.

##### Testing Scenarios:

1. **Basic Functionality Test:** This test verifies the fundamental features of the chat client, including user authentication, joining channels, sending messages, and receiving messages from the server. Input files were created to simulate these actions and ensure that the application behaves as expected.

2. **Error Handling Test:** Error handling is critical for providing a smooth user experience. Input files containing erroneous commands and unexpected server responses were used to validate the application's error handling mechanisms. The chat client should gracefully handle errors and provide informative feedback to the user.

3. **Performance Test:** Performance testing assesses the chat client's responsiveness and scalability under different load conditions. Input files with a large number of concurrent user interactions were used to stress-test the application and identify potential bottlenecks or performance issues.
##### Testing Input File:

During automated testing, the chat client was fed input scenarios from the following file:

- `testing_input.txt`: This file contains a series of predefined input commands and expected server responses, allowing for comprehensive testing of the application's functionality.

    ```plaintext
    /join Hello
    Test Massage For Error
    /auth Login Test1 (Key)
    Hello 1
    /rename Test2
    Hello 2
    /rename Test3
    Hello 3
    ```

##### Expected Output:

Upon processing the input commands from the testing file, the application is expected to produce the following output on the console:

```plaintext
ERR: You are not Authorized.
ERR: You are not Authorized.
Success: Authentication successful.
Server: Test1 joined discord.general.
```
    
Reference Server:

```plaintext
Test1 joined discord.general.
Test1 : Hello 1
Test2 : Hello 2
Test3 : Hello 3
Test3 left discord.general.
```
#### Manual Verification with Wireshark

In addition to automated testing, manual verification was conducted using Wireshark, a network protocol analyzer. Wireshark captures and analyzes network traffic, allowing developers to inspect packets exchanged between the chat client and the server in real-time.

##### Wireshark Testing Process:

1. **Packet Capture:** Wireshark was used to capture network packets during the interaction between the chat client and the server. This includes packets sent and received over TCP and UDP connections.

2. **Protocol Analysis:** The captured packets were analyzed to ensure compliance with the TCP and UDP protocols. Wireshark provides detailed insights into packet headers, payload contents, and protocol behavior, enabling developers to diagnose network-related issues.

3. **Message Verification:** Each message exchanged between the chat client and the server was verified to ensure its integrity, correctness, and adherence to the chat protocol specifications. Wireshark's packet inspection capabilities were instrumental in identifying any anomalies or discrepancies in the transmitted data.

#### Conclusion of testing📣

Testing is an integral part of the software development lifecycle, ensuring the reliability, stability, and performance of the IPK24 Chat Client application. By combining automated testing with manual verification using Wireshark, developers can confidently validate the application's functionality and address any potential issues before deployment.

The thorough testing process employed for the IPK24 Chat Client demonstrates a commitment to quality and reliability, enhancing the overall user experience and satisfaction with the application.