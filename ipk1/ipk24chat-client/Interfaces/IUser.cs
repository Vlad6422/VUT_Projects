using System;
using System.Collections.Generic;
using System.Linq;
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

        void Authenticate();
        void JoinChannel();
        void ChangeDisplayName();
    }
}
