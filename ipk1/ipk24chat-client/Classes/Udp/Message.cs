using System.Text;

namespace ipk24chat_client.Classes.Udp
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
