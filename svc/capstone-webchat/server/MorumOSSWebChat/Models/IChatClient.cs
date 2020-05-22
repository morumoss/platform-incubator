using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MorumOSSWebChat.Models
{
    public interface IChatClient
    {
        Task SendMessage(string user, string message);
        Task ReceiveMessage(string user, string message, DateTime time);
        Task LeaveChat(string user);
        Task GetUserSessionInfo(string username, string sessionId);
        Task ViewUserInfo(string username);
        Task ReceiveUserInfo(string username, bool isAdmin, bool isModerator, bool isBlocked);
        Task ViewAllUsers();
        Task ReceiveAllUsers(List<string> users);
        Task AdminDeletesMessage(string username, string message);
        Task DeleteMessage(string username, string message);
        Task AdminModsUser(string username);
        Task AdminBlocksUser(string username);
        Task BlockUser(string connectionId, string username);
        Task ToggleLockChat();
        Task AdminClearsChat();
        Task ClearChat();
        Task AddFilter(string filter);
    }
}
