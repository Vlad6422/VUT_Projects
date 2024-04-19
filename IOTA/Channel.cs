using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IOTA
{
    public class Channel
    {
        public string ChannelId { get; }
        public List<TcpClient> ConnectedUsersTcp { get; }
        public List<UdpClient> ConnectedUsersUdp { get; }
        public Channel(string channelId)
        {
            ChannelId = channelId;
            ConnectedUsersTcp = new List<TcpClient>();
            ConnectedUsersUdp = new List<UdpClient>();
        }




        public void BroadcastMSG(string Message, EndPoint clientEndPoint)
        {

            foreach (var user in ConnectedUsersTcp)
            {
                if (user.Client.RemoteEndPoint != null && user.Client.RemoteEndPoint.ToString() != clientEndPoint.ToString()) // Exclude the sender
                {
                    // Simulate sending the message to the user (replace with actual sending logic)
                    byte[] responseMsg = Encoding.ASCII.GetBytes(Message);
                    user.GetStream().WriteAsync(responseMsg, 0, responseMsg.Length);
                }
            }
        }



    }
}




