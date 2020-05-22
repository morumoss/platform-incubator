using System.Collections.Generic;

namespace survey_polling.api.Models
{
    /// <summary>
    /// A Question in a Poll
    /// </summary>
    public class Question
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Title { get; set; }
     
        public Poll Poll { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
