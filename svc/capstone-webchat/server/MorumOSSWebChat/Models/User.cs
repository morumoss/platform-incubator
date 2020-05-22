namespace MorumOSSWebChat.Models
{
    public class User
    {
        public string connectionId;
        public string username;
        public string session;
        public bool isAdmin;
        public bool isBlocked = false;
        public bool isModerator = false;
    }
}
