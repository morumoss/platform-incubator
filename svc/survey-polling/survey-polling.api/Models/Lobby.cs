using System.Collections.Generic;

namespace survey_polling.api.Models
{
    public class Lobby
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Pin { get; set; }
        public int? ActiveQuestionId { get; set; }
        public bool HasStarted { get; set; }

        public Poll Poll { get; set; }
        public Question ActiveQuestion { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
