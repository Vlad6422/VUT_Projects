using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Classes.Udp
{
    public class ConfirmMessage
    {
        public byte MessageType { get; set; } = 0x00;
        public ushort RefMessageID { get; set; }
        public ConfirmMessage(ushort RefMessageID)
        {
            this.RefMessageID = RefMessageID;
        }
        public byte[] GET()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(MessageType);
            bytes.AddRange(BitConverter.GetBytes(RefMessageID));
            return bytes.ToArray();
        }

    }

    public class ReplyMessage
    {
        public byte MessageType { get; set; } = 0x01;
        public ushort MessageID { get; set; }
        public byte Result { get; set; }
        public ushort RefMessageID { get; set; }
        public string MessageContents { get; set; }
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

    public class AuthMessage
    {
        public byte MessageType { get; set; } = 0x02;
        public ushort MessageID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Secret { get; set; }
        public AuthMessage(ushort MessageID, string Username, string DisplayName, string Secret)
        {
            this.MessageID = MessageID;
            this.Username = Username;
            this.DisplayName = DisplayName;
            this.Secret = Secret;
        }
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

    public class JoinMessage
    {
        public byte MessageType { get; set; } = 0x03;
        public ushort MessageID { get; set; }
        public string ChannelID { get; set; }
        public string DisplayName { get; set; }
        public JoinMessage(ushort messageID, string channelID, string displayName)
        {
            MessageID = messageID;
            ChannelID = channelID;
            DisplayName = displayName;
        }
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

    public class MsgMessage
    {
        public byte MessageType { get; set; } = 0x04;
        public ushort MessageID { get; set; }
        public string DisplayName { get; set; }
        public string MessageContents { get; set; }
        public MsgMessage(byte[] buff)
        {
            MessageID = BitConverter.ToUInt16(buff, 1);
            int displayNameLength = Array.IndexOf(buff, (byte)0, 3) - 3;
            DisplayName = Encoding.ASCII.GetString(buff, 3, displayNameLength);
            int messageContentsLength = Array.IndexOf(buff, (byte)0, 3 + displayNameLength + 1) - (3 + displayNameLength + 1);
            MessageContents = Encoding.ASCII.GetString(buff, 3 + displayNameLength + 1, messageContentsLength);
        }

    }

    public class ErrMessage
    {
        public byte MessageType { get; set; } = 0xFE;
        public ushort MessageID { get; set; }
        public string DisplayName { get; set; }
        public string MessageContents { get; set; }
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

    public class ByeMessage
    {
        public byte MessageType { get; set; } = 0xFF;
        public ushort MessageID { get; set; }
    }
}
