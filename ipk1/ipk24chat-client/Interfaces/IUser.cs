namespace ipk24chat_client.Interfaces
{
      public interface IUser
    {
        void Start();
        void Stop();

        void Authenticate();
        void JoinChannel(string channelName);
        void SendMessage(string message);
        string RecieveMessage();
        
    }
}
