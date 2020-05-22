using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_polling.api.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace survey_polling.api.Data
{
    /// <summary>
    /// Extension class for the PollContext that contains 
    /// methods/functions for querying against the database.
    /// </summary>
    public static class PollContextExtensions
    {
        /// <summary>
        /// Gets the number of users in a specific lobby.
        /// </summary>
        /// <param name="pollContext">The database context.</param>
        /// <param name="lobbyPin">The lobbies pin code.</param>
        /// <returns>The number of users as an integer.</returns>
        public static async Task<int> GetLobbyUserCountAsync(this PollContext pollContext, string lobbyPin)
        {
            try
            {
                var lobby = await pollContext.Lobbies
                    .Include(l => l.Users)
                    .FirstOrDefaultAsync(l => l.Pin == lobbyPin);

                return lobby.Users.Where(u => u.IsHost == false).Count();
            }
            catch (SqlException sqlException)
            {
                Debug.WriteLine(sqlException.ToString());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }

            return await Task.FromException<int>(new Exception("Unable to count users for a lobby with a pin of: " + lobbyPin));
        }

        /// <summary>
        /// Gets the number of votes in a specific lobby for the active question.
        /// </summary>
        /// <param name="pollContext">The database context.</param>
        /// <param name="lobbyPin">The lobbies pin code.</param>
        /// <returns>The number of votes as an integer.</returns>
        public static async Task<int> GetLobbyVoteCountAsync(this PollContext pollContext, string lobbyPin)
        {
            try
            {
                var lobby = await pollContext.Lobbies
                    .FirstOrDefaultAsync(l => l.Pin == lobbyPin);

                return await pollContext.Votes
                    .Where(v => v.QuestionId == lobby.ActiveQuestionId)
                    .CountAsync();
            }
            catch (SqlException sqlException)
            {
                Debug.WriteLine(sqlException.ToString());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }

            return await Task.FromException<int>(new Exception("Unable to count votes for a lobby with a pin of: " + lobbyPin));
        }

        /// <summary>
        /// Gets the number of votes for a specific question.
        /// </summary>
        /// <param name="pollContext">The database context.</param>
        /// <param name="questionId">The question being queried.</param>
        /// <returns>The number of votes as an integer.</returns>
        public static async Task<int[]> GetQuestionVoteCountAsync(this PollContext pollContext, string lobbyPin, int questionId)
        {
            try
            {
                var lobby = await pollContext.Lobbies
                       .Include(l => l.Users)
                       .Include(l => l.ActiveQuestion)
                            .ThenInclude(q => q.Options)
                       .FirstOrDefaultAsync(l => l.Pin == lobbyPin);

                Option[] options = new Option[lobby.ActiveQuestion.Options.Count];

                lobby.ActiveQuestion.Options.CopyTo(options, 0);

                int[] votes = new int[options.Length];

                for (int i = 0; i < options.Length; i++)
                {
                    votes[i] = await pollContext.Votes
                        .CountAsync(v => v.OptionId == options[i].Id);
                }

                return votes;
            }
            catch (SqlException sqlException)
            {
                Debug.WriteLine(sqlException.ToString());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }

            return await Task.FromException<int[]>(new Exception("Unable to count votes for a question with an id of: " + questionId));
        }

        /// <summary>
        /// Gets the next question in a lobby/poll.
        /// </summary>
        /// <param name="pollContext">The database context.</param>
        /// <param name="lobbyPin">The lobbies pin code.</param>
        /// <returns>The next question in the poll.</returns>
        public static async Task<Question> GetNextQuestionAsync(this PollContext pollContext, string lobbyPin)
        {
            try
            {
                var lobby = await pollContext.Lobbies
                    .Include(l => l.Poll)
                        .ThenInclude(p => p.Questions)
                    .FirstOrDefaultAsync(l => l.Pin == lobbyPin);

                var questions = lobby.Poll.Questions.ToList();

                Question question = new Question();

                // No active question? Grab the first record, 
                // else get the next record in the collection
                if (lobby.ActiveQuestionId == null)
                {
                    questions.FirstOrDefault();

                }
                else
                {
                    question = questions.SkipWhile(q => q.Id != lobby.ActiveQuestionId).Skip(1).FirstOrDefault();
                }

                return question;
            }
            catch (SqlException sqlException)
            {
                Debug.WriteLine(sqlException.ToString());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }

            return await Task.FromException<Question>(new Exception("Unable to get the next question in a lobby with a pin of: " + lobbyPin));
        }
    }
}
