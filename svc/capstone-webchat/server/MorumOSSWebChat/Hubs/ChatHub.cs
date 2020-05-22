using Microsoft.AspNetCore.SignalR;
using MorumOSSWebChat.Models;
using MorumOSSWebChat.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MorumOSSWebChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private static List<string> filteredWords = null;

        public override async Task OnConnectedAsync()
        {
            User user = new User();
            user.connectionId = Context.ConnectionId;
            if (UserRepository.AddUser(user))
            {
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            User user = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (user != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.session);
                await Clients.Group(user.session).LeaveChat(user.username);
                UserRepository.RemoveUser(user);
                if (!UserRepository.SessionHasUsers(user.session))
                {
                    SessionRepository.RemoveSession(user.session);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (userFound != null && userFound.username != null && userFound.isBlocked == false)
            {
                if (userFound.isAdmin || !SessionRepository.IsSessionLocked(userFound.session))
                {
                    if (filteredWords == null) { filteredWords = GetDefaultFilteredWords(); }
                    await Clients.Group(userFound.session).ReceiveMessage(userFound.username, FilterMessage(message, userFound.session), DateTime.Now);
                }
            }
        }

        public async Task GetUserSessionInfo(string username, string sessionId)
        {
            User userFound = UserRepository.UpdateUser(Context.ConnectionId, username, sessionId);
            if (userFound != null)
            {
                await Groups.AddToGroupAsync(userFound.connectionId, userFound.session);
                userFound.isAdmin = SessionRepository.AddSession(userFound.session); // Will set the first user in a session to the administrator
            }
        }

        public async Task ViewUserInfo(string username)
        {
            User currentUser = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (currentUser != null && currentUser.session != null)
            {
                User findUser = UserRepository.users.FirstOrDefault(x => x.session == currentUser.session && x.username.ToLower() == username.ToLower());
                await Clients.Client(Context.ConnectionId).ReceiveUserInfo(findUser.username, findUser.isAdmin, findUser.isBlocked, findUser.isModerator);
            }
        }

        public async Task GetAllUsers()
        {
            User currentUser = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (currentUser != null && currentUser.session != null)
            {
                List<string> usernames = new List<string>();
                foreach (User user in UserRepository.users.Where(x => x.session == currentUser.session))
                {
                    usernames.Add(user.username);
                }
                await Clients.Client(Context.ConnectionId).ReceiveAllUsers(usernames);
            }
        }

        #region "Admin Functionality"

        public async Task AdminBlocksUser(string username)
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (UserRepository.BlockUser(Context.ConnectionId, username) && userFound != null) 
            {
                await Clients.Group(userFound.session).BlockUser(userFound.connectionId, username);
            }
        }

        public async Task AdminClearsChat()
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (userFound != null && (userFound.isAdmin || userFound.isModerator))
            {
                await Clients.Group(userFound.session).ClearChat();
            }
        }

        public async Task AdminDeletesMessage(string username, string message)
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (userFound != null && (userFound.isAdmin || userFound.isModerator))
            {
                await Clients.Group(userFound.session).DeleteMessage(username, message);
            }
        }

        public async Task AdminModsUser(string username)
        {
            UserRepository.ModUser(Context.ConnectionId, username);
        }

        public async Task ToggleLockChat()
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (userFound != null && (userFound.isAdmin || userFound.isModerator))
            {
                bool isLocked = SessionRepository.ToggleSessionLock(userFound.session);
                if (isLocked)
                {
                    await SendMessage(userFound.username, "CHAT HAS BEEN LOCKED");
                }
                else
                {
                    await SendMessage(userFound.username, "CHAT HAS BEEN UNLOCKED");
                }
            }
        }

        #endregion

        #region "Filter Functionality"
        public async Task AddFilter(string filter)
        {
            User userFound = UserRepository.users.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (userFound.session != null && (userFound.isAdmin || userFound.isModerator))
            {
                Session sessionFound = SessionRepository.sessions.FirstOrDefault(s => s.sessionId == userFound.session);
                foreach (string item in filter.Split(","))
                {
                    if ((item != null) && (item != ""))
                    {
                        sessionFound.filteredWords.Add(item.Trim().ToLower());
                    }
                }
            }
        }
        static public string FilterMessage(string originalMessage, string session)
        {
            string filteredMessage = originalMessage;
            string lowercaseMessage = originalMessage.ToLower();
            foreach (string filter in filteredWords)
            {
                if (lowercaseMessage.Contains(filter)) { filteredMessage = filteredMessage.Replace(filter, "[Bleep]", StringComparison.OrdinalIgnoreCase); };
            }
            Session userSession = SessionRepository.sessions.FirstOrDefault(s => s.sessionId == session);
            if (userSession != null)
            {
                foreach (string filter in userSession.filteredWords)
                {
                    if (lowercaseMessage.Contains(filter)) { filteredMessage = filteredMessage.Replace(filter, "[Bleep]", StringComparison.OrdinalIgnoreCase); };
                }
            }
            return filteredMessage;
        }

        static public List<string> GetDefaultFilteredWords()
        {
            List<string> words = new List<string>();

            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://raw.githubusercontent.com/RobertJGabriel/Google-profanity-words/master/list.txt");
            StreamReader reader = new StreamReader(stream);
            String content = reader.ReadToEnd();
            foreach (string word in content.Split("\n"))
            {
                if((word != null) && (word != ""))
                {
                    words.Add(word);
                }
            }

            return words;
        }

        #endregion
    }
}
