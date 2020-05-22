using System.Collections.Generic;

namespace survey_polling.api.Models
{
    /// <summary>
    /// A Poll that is created and ran by a host (Instructor).
    /// </summary>
    public class Poll
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string Title { get; set; }

        public User Host { get; set; }
        public ICollection<Lobby> Lobbies { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
