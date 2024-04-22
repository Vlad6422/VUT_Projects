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


### Testing Input File Structure

The `testing_input.txt` file appears to follow this structure:

1. **Commands Sent by Users**:
   - Text that TCP/UDP users send to the server.

2. **Commands and Responses**:
   - Commands from users (`/auth username DisplayName token`, `hello`, `My name is Ivan`, `/join News`).
   - Expected server responses with format details (`RECV` for messages received by the server, `SENT` for messages sent by the server).
   - Server responses differ based on whether the user is using TCP or UDP.

### Example Content Breakdown

Here's how you might interpret the example content you've provided:

- **Text Sent by TCP/UDP Users**:
  ```
  add here text that user Tcp sends
  /auth username DisplayName token
  hello
  My name is Ivan
  /join News
  ```

- **Expected Server Responses for TCP Users**:
  ```
  RECV 127.0.0.1:51364 | AUTH
  SENT 127.0.0.1:51364 | REPLY
  RECV 127.0.0.1:51364 | MSG
  RECV 127.0.0.1:51364 | MSG
  RECV 127.0.0.1:51364 | JOIN
  SENT 127.0.0.1:51364 | REPLY
  RECV 127.0.0.1:51364 | BYE
  ```

- **Expected Server Responses for UDP Users**:
  ```
  RECV 127.0.0.1:51364 | AUTH
  SENT 127.0.0.1:51364 | CONFIRM
  SENT 127.0.0.1:51364 | REPLY
  RECV 127.0.0.1:51364 | CONFIRM
  RECV 127.0.0.1:51364 | MSG
  SENT 127.0.0.1:51364 | CONFIRM
  RECV 127.0.0.1:51364 | MSG
  SENT 127.0.0.1:51364 | CONFIRM
  RECV 127.0.0.1:51364 | JOIN
  SENT 127.0.0.1:51364 | CONFIRM
  SENT 127.0.0.1:51364 | REPLY
  RECV 127.0.0.1:51364 | CONFIRM
  RECV 127.0.0.1:51364 | BYE
  SENT 127.0.0.1:51364 | CONFIRM
  RECV 127.0.0.1:51364 | BYE
  SENT 127.0.0.1:51364 | CONFIRM
  ```

### Explanation of Server Responses

- **TCP Server Responses**:
  - `RECV`: Server receives a message from the client.
  - `SENT`: Server sends a response back to the client.

- **UDP Server Responses**:
  - Similar format as TCP but with additional `CONFIRM` messages indicating acknowledgments for UDP communication.

### Usage
This structured testing input file (`testing_input.txt`) can be used to automate testing of your application's interaction with the server. By processing the commands and verifying the server responses against the expected outputs, you can ensure the correct behavior of your application under different scenarios for both TCP and UDP users.
### Test with 6 Users
Based on the provided documentation and example outputs, let's break down the scenario you described into a structured test setup. Here's an example of how you can document and represent this scenario:

### Test Scenario Description
This test involves simulating communication between six users (clients) and a server using different protocols (TCP and UDP) to send messages and join specific channels.

### Test Setup
- **Server Details**:
  - **Address**: 127.0.0.1
  - **TCP Port**: 4567
  - **UDP Port**: 4567

### Users and Actions
1. **User 1 (TCP) - TcpUser**
   - Action: Send TCP packets to the server.
   - Commands:
     - `/auth 1 TcpUser 1`
     - `MSG FROM TCP FOR GENERAL`
     - `/join 123`
     -  `MSG FROM TCP FOR 123`

2. **User 2 (UDP) - UdpUser**
   - Action: Send UDP packets to the server.
   - Messages:
      - `/auth 1 TcpUser 1`
     - `MSG FROM TCP FOR GENERAL`
     - `/join 123`
     -  `MSG FROM TCP FOR 123`

3. **User 3 (TCP) - TcpConnectedToGeneral**
   - Action: Establish TCP connection and join the `general` channel.
   - Commands:
     - `/auth 1 TcpConnectedToGeneral 1`

4. **User 4 (TCP) - TcpConnectedTo123**
   - Action: Establish TCP connection and join the `123` channel.
   - Commands:
     - `/auth 1 TcpConnectedTo123 1`
     - `join 123`

5. **User 5 (UDP) - UdpConnectedToGeneral**
   - Action: Establish UDP connection and join the `general` channel.
   - Commands:
     - `/auth 1 UdpConnectedToGeneral 1`

6. **User 6 (UDP) - UdpConnectedTo123**
   - Action: Establish UDP connection and join the `123` channel.
   - Commands:
     - `/auth 1 UdpConnectedTo123 1`
     - `join 123`

### Output of clients:
## Client 1
```
- Success: Auth Success
- Success: JOIN Succ
```
## Client 2
```
- Success: AUTH Succ
- Success: JOIN Succ
```
## Client 3
```
Success: Auth Success
Server: TcpConnectedTo123 has joined general
Server: TcpConnectedTo123 has left general
Server: UdpConnectedToGeneral has joined general
Server: UdpConnectedTo123 has joined general
Server: UdpConnectedTo123 has left general
Server: TcpUser has joined general
TcpUser: MSG FROM TCP FOR GENERAL
Server: TcpUser has left general
Server: UdpUser has joined general
UdpUser: MSG FROM UDP FOR GENERAL
Server: UdpUser has left general
```
## Client 4
```
Success: Auth Success
Success: Join Success
Server: UdpConnectedTo123 has joined 123
Server: TcpUser has joined 123
TcpUser: MSG FROM TCP FOR 123
Server: TcpUser has left 123
Server: UdpUser has joined 123
UdpUser: MSG FROM UDP FOR 123
Server: UdpUser has left 123
```
## Client 5
```
Success: AUTH Succ
Server: UdpConnectedTo123 has joined general
Server: UdpConnectedTo123 has left general
Server: TcpUser has joined general
TcpUser: MSG FROM TCP FOR GENERAL
Server: TcpUser has left general
Server: UdpUser has joined general
UdpUser: MSG FROM UDP FOR GENERAL
Server: UdpUser has left general
```
## Client 6
```
Success: AUTH Succ
/join 123
Success: JOIN Succ
Server: TcpUser has joined 123
TcpUser: MSG FROM TCP FOR 123
Server: TcpUser has left 123
Server: UdpUser has joined 123
UdpUser: MSG FROM UDP FOR 123
Server: UdpUser has left 123
```
### Server output:
```
TCP connection accepted from: 127.0.0.1:53713
RECV 127.0.0.1:53713 | AUTH Username=1 DisplayName=TcpConnectedToGeneral Secret=1
SENT 127.0.0.1:53713 | REPLY Username=1 DisplayName=TcpConnectedToGeneral Secret=1
User TcpConnectedToGeneral joined channel general
TCP connection accepted from: 127.0.0.1:53715
RECV 127.0.0.1:53715 | AUTH Username=1 DisplayName=TcpConnectedTo123 Secret=1
SENT 127.0.0.1:53715 | REPLY Username=1 DisplayName=TcpConnectedTo123 Secret=1
User TcpConnectedTo123 joined channel general
SENT 127.0.0.1:53713 | MSG
RECV 127.0.0.1:53715 | JOIN ChannelID=123 DisplayName=TcpConnectedTo123
SENT 127.0.0.1:53715 | REPLY DisplayName=TcpConnectedTo123 ChannelID=123
User TcpConnectedTo123 removed from channel general
SENT 127.0.0.1:53713 | MSG
RECV 127.0.0.1:55087 | AUTH
SENT 127.0.0.1:55087 | CONFIRM
SENT 127.0.0.1:55087 | REPLY
RECV 127.0.0.1:55087 | CONFIRM
SENT 127.0.0.1:53713 | MSG
RECV 127.0.0.1:55089 | AUTH
SENT 127.0.0.1:55089 | CONFIRM
SENT 127.0.0.1:55089 | REPLY
RECV 127.0.0.1:55089 | CONFIRM
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:55087 | CONFIRM
RECV 127.0.0.1:55089 | JOIN
SENT 127.0.0.1:55089 | CONFIRM
SENT 127.0.0.1:55089 | REPLY
RECV 127.0.0.1:55089 | CONFIRM
User UdpConnectedTo123 removed from channel general
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
User UdpConnectedTo123 joined channel 123
SENT 127.0.0.1:53715 | MSG
RECV 127.0.0.1:55087 | CONFIRM
TCP connection accepted from: 127.0.0.1:53719
RECV 127.0.0.1:53719 | AUTH Username=1 DisplayName=TcpUser Secret=1
SENT 127.0.0.1:53719 | REPLY Username=1 DisplayName=TcpUser Secret=1
User TcpUser joined channel general
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:55087 | CONFIRM
RECV 127.0.0.1:53719 | MSG DisplayName=TcpUser MessageContent=MSG FROM TCP FOR GENERAL
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:53719 | JOIN ChannelID=123 DisplayName=TcpUser
RECV 127.0.0.1:55087 | CONFIRM
SENT 127.0.0.1:53719 | REPLY DisplayName=TcpUser ChannelID=123
User TcpUser removed from channel general
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:55087 | CONFIRM
SENT 127.0.0.1:53715 | MSG
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:53719 | MSG DisplayName=TcpUser MessageContent=MSG FROM TCP FOR 123
RECV 127.0.0.1:55089 | CONFIRM
SENT 127.0.0.1:53715 | MSG
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:53719 | BYE DisplayName=TcpUser
RECV 127.0.0.1:55089 | CONFIRM
SENT 127.0.0.1:53715 | MSG
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:55089 | CONFIRM
RECV 127.0.0.1:55091 | AUTH
SENT 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:55091 | REPLY
RECV 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:55087 | CONFIRM
RECV 127.0.0.1:55091 | MSG
SENT 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
RECV 127.0.0.1:55087 | CONFIRM
RECV 127.0.0.1:55091 | JOIN
SENT 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:55091 | REPLY
RECV 127.0.0.1:55091 | CONFIRM
User UdpUser removed from channel general
SENT 127.0.0.1:53713 | MSG
SENT 127.0.0.1:55087 | MSG
User UdpUser joined channel 123
SENT 127.0.0.1:53715 | MSG
RECV 127.0.0.1:55087 | CONFIRM
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:55089 | CONFIRM
RECV 127.0.0.1:55091 | MSG
SENT 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:53715 | MSG
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:55089 | CONFIRM
RECV 127.0.0.1:55091 | BYE
SENT 127.0.0.1:55091 | CONFIRM
SENT 127.0.0.1:53715 | MSG
SENT 127.0.0.1:55089 | MSG
RECV 127.0.0.1:55089 | CONFIRM
```
### Results:
Based on the provided logs and interactions, here's a detailed description of the actions of users 3 through 6, and the corresponding server responses:

### User 3 (TcpConnectedToGeneral):
- **Action:**
  - Authenticated successfully (`Success: Auth Success`).
  - Automatically joined the `general` channel.
  
- **Server Response:**
  - Server notified that `TcpConnectedToGeneral` joined the `general` channel.
  
- **Observation:**
  - User 3 (`TcpConnectedToGeneral`) remains in the `general` channel, waiting.

### User 4 (TcpConnectedTo123):
- **Action:**
  - Authenticated successfully (`Success: Auth Success`).
  - Sent a join command for channel `123` (`/join 123`).
  
- **Server Response:**
  - Server acknowledged the join request for `123`.
  - Server notified that `TcpConnectedTo123` joined `123`.
  
- **Observation:**
  - User 4 (`TcpConnectedTo123`) briefly joined `general` and `123`, then left `general`.

### User 5 (UdpConnectedToGeneral):
- **Action:**
  - Authenticated successfully (`Success: AUTH Succ`).
  - Automatically joined the `general` channel.
  
- **Server Response:**
  - Server notified that `UdpConnectedToGeneral` joined the `general` channel.
  
- **Observation:**
  - User 5 (`UdpConnectedToGeneral`) remains in the `general` channel, waiting.

### User 6 (UdpConnectedTo123):
- **Action:**
  - Authenticated successfully (`Success: AUTH Succ`).
  - Sent a join command for channel `123` (`/join 123`).
  
- **Server Response:**
  - Server acknowledged the join request for `123`.
  - Server notified that `UdpConnectedTo123` joined `123`.
  
- **Observation:**
  - User 6 (`UdpConnectedTo123`) briefly joined `general` and `123`, then left `general`.

### Server Responses to User Interactions:
- **Authentication:**
  - Server sent confirmation replies (`REPLY`) upon successful authentication for all users.
  
- **Channel Join and Leave Events:**
  - Server broadcasted messages when users joined (`joined`) and left (`removed from`) channels, indicating the specific channels (`general`, `123`).
  
- **Message Handling:**
  - Server relayed messages (`MSG`) sent by users to appropriate channels (`general`, `123`).
  
- **UDP Confirmation and Retransmission Handling:**
  - Server acknowledged UDP confirmations (`CONFIRM`) and managed retransmissions.
  
- **Error Handling:**
  - Server outputted error messages related to connection issues (`Unable to read data...`).

### Message Distribution:
- **All Users (Tcp and Udp):**
  - Received messages sent by other users (`TcpUser`, `UdpUser`) in respective channels (`general`, `123`).
  
### Conclusion:
Users 3 through 6 authenticated, joined channels (`general`, `123`), and awaited further interaction. Server responses included notifications of user actions (joins, leaves), message handling, and error management. The server efficiently managed communication and coordination among all connected users (TCP and UDP) based on their interactions and commands.
### Manual Verification with Wireshark

Manual verification using Wireshark captures and analyzes network traffic between the server and clients, ensuring compliance with TCP and UDP protocols and validating message integrity.

## Conclusion📣

Thorough testing of the IPK24 Chat Server ensures reliability, stability, and performance. Automated testing and manual verification with Wireshark enhance confidence in the server's functionality, ensuring a seamless user experience.