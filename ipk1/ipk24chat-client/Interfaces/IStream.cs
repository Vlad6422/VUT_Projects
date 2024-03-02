using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipk24chat_client.Interfaces
{
     interface IStream
    {
        NetworkStream networkStream {  get; }
    }
}
