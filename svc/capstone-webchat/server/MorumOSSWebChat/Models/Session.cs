using System.Collections.Generic;

namespace MorumOSSWebChat.Models
{
    public class Session
    {
        public string sessionId;
        public bool isLocked = false;
        public List<string> filteredWords = new List<string>();
    }
}
