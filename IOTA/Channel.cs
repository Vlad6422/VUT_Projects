using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using IOTA.Udp;

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
                    Console.WriteLine($"SENT {user.Client.RemoteEndPoint} | MSG");
                }
            }
            string pattern = @"MSG FROM (\w+) IS (.+)";

            // Use Regex to match the pattern in the input string
            Match match = Regex.Match(Message, pattern);


            // Extract "TCP_man" from the first captured group
            string DisplayName = match.Groups[1].Value;

            // Extract "Hello everybody!" from the second captured group
            string Context = match.Groups[2].Value;


            MsgMessage msgMessage = new MsgMessage(1, DisplayName, Context);
            foreach (var user in ConnectedUsersUdp)
            {
                if (user.Client.RemoteEndPoint != null && user.Client.RemoteEndPoint.ToString() != clientEndPoint.ToString()) // Exclude the sender
                {
                    // Simulate sending the message to the user (replace with actual sending logic)
                    user.SendAsync(msgMessage.GET());
                    Console.WriteLine($"SENT {user.Client.RemoteEndPoint} | MSG");
                }
            }
        }



    }
}




