using MorumOSSWebChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MorumOSSWebChat.Repositories
{
    static class UserRepository
    {
        public static List<User> users = new List<User>();

        public static bool AddUser(User user)
        {
            if (users.FirstOrDefault(x => x.connectionId.ToLower() == user.connectionId.ToLower()) != null)
            {
                return false;
            }
            else
            {
                users.Add(user);
                return true;
            }
        }

        public static void RemoveUser(User user)
        {
            User userFound = users.FirstOrDefault(x => x.connectionId.ToLower() == user.connectionId.ToLower());
            if (userFound != null)
            {
                users.Remove(userFound);
            }
        }

        public static User UpdateUser(String connectionId, String username, String sessionId)
        {
            User userFound = users.FirstOrDefault(x => x.connectionId.ToLower() == connectionId.ToLower());
            if (userFound != null)
            {
                userFound.username = username;
                userFound.session = sessionId;
            }
            return userFound;
        }

        public static bool BlockUser(String connectionId, String username)
        {
            User adminUser = users.FirstOrDefault(x => x.connectionId.ToLower() == connectionId.ToLower());
            if (adminUser != null && adminUser.isAdmin)
            {
                User blockUser = users.FirstOrDefault(x => x.session.ToLower() == adminUser.session.ToLower() && x.username.ToLower() == username.ToLower());
                if (blockUser != null)
                {
                    blockUser.isBlocked = !blockUser.isBlocked;
                    return true;
                }
            }
            return false;
        }

        public static void ModUser(String connectionId, String username)
        {
            User adminUser = users.FirstOrDefault(x => x.connectionId.ToLower() == connectionId.ToLower());
            if (adminUser != null && adminUser.isAdmin) 
            {
                User modUser = users.FirstOrDefault(x => x.session.ToLower() == adminUser.session.ToLower() && x.username.ToLower() == username.ToLower());
                if (modUser != null)
                {
                    modUser.isModerator = !modUser.isModerator;
                }
            }
        }

        public static bool SessionHasUsers(String session)
        {
            User userFound = users.FirstOrDefault(x => x.session == session);
            if (userFound != null)
            {
                return true;
            }
            return false;
        }
    }
}
