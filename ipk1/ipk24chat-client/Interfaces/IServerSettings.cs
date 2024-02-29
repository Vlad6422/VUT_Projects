using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Interfaces
{
    internal interface IServerSettings
    {
        string transportProtocol { get; }
        string serverAddress { get; }
        ushort serverPort { get; }
        ushort udpConfirmationTimeout { get; }
        byte maxUdpRetransmissions { get; }
    }
}
