namespace survey_polling.api.Models
{
    /// <summary>
    /// A User within the Poll.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public bool IsHost { get; set; }
    }
}
