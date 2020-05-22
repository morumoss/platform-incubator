using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using survey_polling.api.Data;
using survey_polling.api.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace survey_polling.api.Hubs
{
    /// <summary>
    /// SignalR hub for polling.
    /// </summary>
    public class PollHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructs a PollHub with a required IServiceProvider.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PollHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Sends the connection id to the client that connected
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("userConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Removes the user from the lobby and updated the user count on all clients.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            try
            {
                var user = await pollContext.Users.FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);
                var lobbies = pollContext.Lobbies.Include(l => l.Users);

                Lobby lobby = null;
                await lobbies.ForEachAsync(l => {
                    if (l.Users.Contains(user))
                    {
                        lobby = l;
                        return;
                    }
                });

                pollContext.Attach(lobby);
                lobby.Users.Remove(user);
                pollContext.Entry(lobby).State = EntityState.Modified;

                await pollContext.SaveChangesAsync();
                await Clients.Group(lobby.Pin).SendAsync(PollActions.USER_LEFT, await pollContext.GetLobbyUserCountAsync(lobby.Pin));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Adds a user to a lobby.
        /// </summary>
        /// <param name="pin">The lobby pin.</param>
        public async Task JoinLobby(string pin)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            try
            {
                var lobby = await pollContext.Lobbies.Include(l => l.Users).FirstOrDefaultAsync(l => l.Pin == pin);
                var user = await pollContext.Users.FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);

                if (lobby.Users.Any(u => u.ConnectionId == null))
                {
                    pollContext.Attach(lobby);
                    lobby.Users.Add(user);
                    pollContext.Entry(lobby).State = EntityState.Modified;

                    await pollContext.SaveChangesAsync();
                    await Groups.AddToGroupAsync(Context.ConnectionId, pin);
                    await Clients.Groups(pin).SendAsync(PollActions.USER_JOINED, await pollContext.GetLobbyUserCountAsync(pin));
                }
                else if (lobby.Users.Any(u => u.ConnectionId == Context.ConnectionId))
                {
                    await Clients.Caller.SendAsync(PollActions.USER_ALREADY_JOINED);
                }
                else
                {

                    pollContext.Attach(lobby);
                    lobby.Users.Add(user);
                    pollContext.Entry(lobby).State = EntityState.Modified;

                    await pollContext.SaveChangesAsync();
                    await Groups.AddToGroupAsync(Context.ConnectionId, pin);
                    await Clients.Groups(pin).SendAsync(PollActions.USER_JOINED, await pollContext.GetLobbyUserCountAsync(pin));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Removes a user to a lobby.
        /// </summary>
        /// <param name="pin">The lobby pin.</param>
        public async Task LeaveLobby(string pin)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            try
            {
                var lobby = await pollContext.Lobbies.FirstOrDefaultAsync(l => l.Pin == pin);

                if (!lobby.Users.Any(u => u.ConnectionId == Context.ConnectionId))
                {
                    await Clients.Caller.SendAsync(PollActions.USER_NOT_FOUND);
                }
                else
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, pin);
                    await Clients.Groups(pin).SendAsync(PollActions.USER_LEFT, await pollContext.GetLobbyUserCountAsync(pin));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Tells all users of a specific lobby that the poll has started.
        /// </summary> 
        /// <param name="pin">The lobby pin</param>
        public async Task StartPoll(string pin)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            try
            {
                var lobby = await pollContext.Lobbies.FirstOrDefaultAsync(l => l.Pin == pin);

                pollContext.Attach(lobby);
                lobby.HasStarted = true;
                pollContext.Entry(lobby).State = EntityState.Modified;

                await pollContext.SaveChangesAsync();
                await Clients.Groups(pin).SendAsync(PollActions.POLL_STARTED);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Sends a count of votes for a specific question to all users of a specific lobby.
        /// </summary>
        /// <param name="pin">The lobby pin.</param>
        public async Task SendVoteCount(string pin, int questionId)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            await Clients.Group(pin).SendAsync(PollActions.USER_VOTED, await pollContext.GetQuestionVoteCountAsync(pin, questionId));
        }

        public async Task SendNextQuestion(string pin)
        {
            using var scope = _serviceProvider.CreateScope();
            var pollContext = scope.ServiceProvider.GetRequiredService<PollContext>();

            await Clients.Group(pin).SendAsync(PollActions.POLL_NEXT_QUESTION, await pollContext.GetNextQuestionAsync(pin));
        }
    }
}

