namespace ipk24chat_client.Interfaces
{
      public interface IUser
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
