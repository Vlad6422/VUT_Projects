using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Interfaces
{
    internal interface IUser
    {
        string Username { get; set; }
        string Secret { get; set; }
        string DisplayName { get; set; }
        string ChannelId { get; set; }
        string Message { get; set; }
        void Start();
        void Stop();

        void Authenticate();
        void JoinChannel(string channelName);
        void SendMessage(string message);
        string RecieveMessage();
        void ChangeDisplayName(string newName);
    }
}
