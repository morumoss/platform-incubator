namespace survey_polling.api.Hubs
{
    /// <summary> 
    /// Web Socket actions for the PollHub.
    /// </summary>
    public static class PollActions
    {
        /// <summary>
        /// User joined an active poll
        /// </summary>
        public static readonly string USER_JOINED = "userJoined";

        /// <summary>
        /// User left an active poll
        /// </summary>
        public static readonly string USER_LEFT = "userLeft";

        /// <summary>
        /// User voted on a question.
        /// </summary>
        public static readonly string USER_VOTED = "userVoted";

        /// <summary>
        /// User tried to join a lobby but is already in it.
        /// </summary>
        public static readonly string USER_ALREADY_JOINED = "userAlreadyJoined";
        
        /// <summary>
        /// User not found in lobby.
        /// </summary>
        public static readonly string USER_NOT_FOUND = "userNotFound";

        /// <summary>
        /// Host started a poll.
        /// </summary>
        public static readonly string POLL_STARTED = "pollStarted";

        /// <summary>
        /// Host ended a poll.
        /// </summary>
        public static readonly string POLL_ENDED = "pollEnded";

        /// <summary>
        /// Host clicked the next question button.
        /// </summary>
        public static readonly string POLL_NEXT_QUESTION = "pollNextQuestion";
    }
}
