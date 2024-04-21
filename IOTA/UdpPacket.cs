using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTA
{
        
    
    // Represents a confirmation message
    public class ConfirmMessage
    {
        public byte MessageType { get; set; } = 0x00;
        public ushort RefMessageID { get; set; }

        // Constructor
        public ConfirmMessage(ushort RefMessageID)
        {
            this.RefMessageID = RefMessageID;
        }

        // Method to get the bytes of the message
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(RefMessageID));
            return bytes.ToArray();
        }
    }

    // Represents a reply message
    public class ReplyMessage
    {
        public byte MessageType { get; set; } = 0x01;
        public ushort MessageID { get; set; }
        public byte Result { get; set; }
        public ushort RefMessageID { get; set; }
        public string MessageContents { get; set; }

        public ReplyMessage(ushort MessageId,byte Result,ushort RefMessageID,string MessageContent)
        {
            MessageID = MessageID;
            this.Result = Result;
            this.RefMessageID = RefMessageID;
            MessageContents = MessageContent;
        }
        // Constructor
        public ReplyMessage(byte[] buff)
        {
            MessageType = buff[0];
            MessageID = BitConverter.ToUInt16(buff, 1);
            Result = buff[3];
            RefMessageID = BitConverter.ToUInt16(buff, 4);
            int messageContentsLength = Array.IndexOf(buff, (byte)0, 6) - 6;
            MessageContents = Encoding.ASCII.GetString(buff, 6, messageContentsLength);
        }
        // Method to serialize message into byte array
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(MessageID));
            bytes.Add(Result);
            bytes.AddRange(BitConverter.GetBytes(RefMessageID));
            bytes.AddRange(Encoding.ASCII.GetBytes(MessageContents));
            bytes.Add(0); // Null terminator for MessageContents
            return bytes.ToArray();
        }
    }

    // Represents an authentication message
    public class AuthMessage
    {
        public byte MessageType { get; set; } = 0x02;
        public ushort MessageID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Secret { get; set; }

        // Constructor
        public AuthMessage(ushort MessageID, string Username, string DisplayName, string Secret)
        {
            this.MessageID = MessageID;
            this.Username = Username;
            this.DisplayName = DisplayName;
            this.Secret = Secret;
        }
        // Constructor to parse from byte array
        public AuthMessage(byte[] packet)
        {
            if (packet == null || packet.Length < 7)
            {
                throw new ArgumentException("Invalid packet format");
            }

            // Ensure the packet starts with 0x02 (start of message marker)
            if (packet[0] != 0x02)
            {
                throw new ArgumentException("Invalid start of packet marker");
            }

            // Read MessageID (2 bytes)
            this.MessageID = BitConverter.ToUInt16(packet, 1);

            // Find Username end (0 terminator)
            int usernameEndIndex = Array.IndexOf(packet, (byte)0, 3); // Start searching after MessageID
            if (usernameEndIndex == -1)
            {
                throw new ArgumentException("Invalid packet format - Username terminator not found");
            }
            this.Username = Encoding.ASCII.GetString(packet, 3, usernameEndIndex - 3);

            // Find DisplayName end (0 terminator) after Username
            int displayNameStartIndex = usernameEndIndex + 1;
            int displayNameEndIndex = Array.IndexOf(packet, (byte)0, displayNameStartIndex);
            if (displayNameEndIndex == -1)
            {
                throw new ArgumentException("Invalid packet format - DisplayName terminator not found");
            }
            this.DisplayName = Encoding.ASCII.GetString(packet, displayNameStartIndex, displayNameEndIndex - displayNameStartIndex);

            // Find Secret end (0 terminator) after DisplayName
            int secretStartIndex = displayNameEndIndex + 1;
            int secretEndIndex = Array.IndexOf(packet, (byte)0, secretStartIndex);
            if (secretEndIndex == -1)
            {
                throw new ArgumentException("Invalid packet format - Secret terminator not found");
            }
            this.Secret = Encoding.ASCII.GetString(packet, secretStartIndex, secretEndIndex - secretStartIndex);
        }


        // Method to get the bytes of the message
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(MessageID));
            bytes.AddRange(Encoding.ASCII.GetBytes(Username));
            bytes.Add(0);
            bytes.AddRange(Encoding.ASCII.GetBytes(DisplayName));
            bytes.Add(0);
            bytes.AddRange(Encoding.ASCII.GetBytes(Secret));
            bytes.Add(0);
            return bytes.ToArray();
        }
    }

    // Represents a join message
    public class JoinMessage
    {
        public byte MessageType { get; set; } = 0x03;
        public ushort MessageID { get; set; }
        public string ChannelID { get; set; }
        public string DisplayName { get; set; }

        // Constructor
        public JoinMessage(ushort messageID, string channelID, string displayName)
        {
            MessageID = messageID;
            ChannelID = channelID;
            DisplayName = displayName;
        }
        public JoinMessage(byte[] data)
        {
            // Check if the data array is at least the minimum expected length
            if (data == null || data.Length < 5) // 5 bytes minimum required (1 + 2 + 1 + 1)
            {
                throw new ArgumentException("Invalid byte array length for JoinMessage");
            }

            // Read MessageType (first byte), assuming it's always 0x03
            this.MessageType = data[0];

            // Read MessageID (next 2 bytes as ushort)
            this.MessageID = BitConverter.ToUInt16(data, 1);

            // Read ChannelID (string terminated by null byte)
            int channelIdEndIndex = Array.IndexOf<byte>(data, 0, 3); // Start looking from index 3
            if (channelIdEndIndex == -1)
            {
                throw new ArgumentException("Invalid byte array format for ChannelID");
            }
            this.ChannelID = Encoding.ASCII.GetString(data, 3, channelIdEndIndex - 3);

            // Read DisplayName (string starting after ChannelID, terminated by null byte)
            int displayNameStartIndex = channelIdEndIndex + 1;
            int displayNameEndIndex = Array.IndexOf<byte>(data, 0, displayNameStartIndex);
            if (displayNameEndIndex == -1)
            {
                throw new ArgumentException("Invalid byte array format for DisplayName");
            }
            this.DisplayName = Encoding.ASCII.GetString(data, displayNameStartIndex, displayNameEndIndex - displayNameStartIndex);
        }
        // Method to get the bytes of the message
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(MessageID));
            bytes.AddRange(Encoding.ASCII.GetBytes(ChannelID));
            bytes.Add(0);
            bytes.AddRange(Encoding.ASCII.GetBytes(DisplayName));
            bytes.Add(0);
            return bytes.ToArray();
        }
    }

    // Represents a message
    public class MsgMessage
    {
        public byte MessageType { get; set; } = 0x04;
        public ushort MessageID { get; set; }
        public string DisplayName { get; set; }
        public string MessageContents { get; set; }

        // Constructor
        public MsgMessage(byte[] buff)
        {
            MessageID = BitConverter.ToUInt16(buff, 1);
            int displayNameLength = Array.IndexOf(buff, (byte)0, 3) - 3;
            DisplayName = Encoding.ASCII.GetString(buff, 3, displayNameLength);
            int messageContentsLength = Array.IndexOf(buff, (byte)0, 3 + displayNameLength + 1) - (3 + displayNameLength + 1);
            MessageContents = Encoding.ASCII.GetString(buff, 3 + displayNameLength + 1, messageContentsLength);
        }

        // Constructor overload
        public MsgMessage(ushort MessageId, string DisplayName, string MessageContent)
        {
            MessageID = MessageId;
            this.DisplayName = DisplayName;
            MessageContents = MessageContent;
        }

        // Method to get the bytes of the message
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(MessageID));
            bytes.AddRange(Encoding.ASCII.GetBytes(DisplayName));
            bytes.Add(0);
            bytes.AddRange(Encoding.ASCII.GetBytes(MessageContents));
            bytes.Add(0);
            return bytes.ToArray();
        }
    }

    // Represents an error message
    public class ErrMessage
    {
        public byte MessageType { get; set; } = 0xFE;
        public ushort MessageID { get; set; }
        public string DisplayName { get; set; }
        public string MessageContents { get; set; }

        // Constructor
        public ErrMessage(byte[] buff)
        {
            MessageType = buff[0];
            MessageID = BitConverter.ToUInt16(buff, 1);
            int displayNameLength = Array.IndexOf(buff, (byte)0, 3) - 3;
            DisplayName = Encoding.ASCII.GetString(buff, 3, displayNameLength);
            int messageContentsLength = Array.IndexOf(buff, (byte)0, 3 + displayNameLength + 1) - (3 + displayNameLength + 1);
            MessageContents = Encoding.ASCII.GetString(buff, 3 + displayNameLength + 1, messageContentsLength);
        }
    }

    // Represents a goodbye message
    public class ByeMessage
    {
        public byte MessageType { get; set; } = 0xFF;
        public ushort MessageID { get; set; }

        // Constructor
        public ByeMessage(ushort MessageId)
        {
            MessageID = MessageId;
        }

        // Method to get the bytes of the message
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(MessageID));
            return bytes.ToArray();
        }
    }
}
