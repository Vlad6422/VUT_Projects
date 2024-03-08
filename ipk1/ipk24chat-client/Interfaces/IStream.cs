using System.Net.Sockets;


namespace ipk24chat_client.Interfaces
{
     public interface IStream
    {
        NetworkStream networkStream {  get; }
    }
}
