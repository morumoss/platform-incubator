namespace survey_polling.api.Models
{
    /// <summary>
    /// Contains information for which Vote corresponds to which Poll and Question.
    /// </summary>
    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int UserId { get; set; }

        public Poll Poll { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }
        public User User { get; set; }
    }
}
