namespace survey_polling.api.Models
{
    /// <summary>
    /// A selectable Option in a Poll.
    /// </summary>
    public class Option
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }

        public Poll Poll { get; set; }
        public Question Question { get; set; }
    }
}
