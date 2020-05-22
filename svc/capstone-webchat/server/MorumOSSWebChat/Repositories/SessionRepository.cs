using MorumOSSWebChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MorumOSSWebChat.Repositories
{
    public class SessionRepository
    {
        public static List<Session> sessions = new List<Session>();

        public static bool AddSession(String session)
        {
            Session sessionFound = sessions.FirstOrDefault(x => x.sessionId == session);
            if (sessionFound == null)
            {
                sessionFound = new Session();
                sessionFound.sessionId = session;
                sessions.Add(sessionFound);
                return true;
            }
            return false;
        }

        public static bool ToggleSessionLock(String session)
        {
            Session sessionFound = sessions.FirstOrDefault(x => x.sessionId == session);
            if (sessionFound != null)
            {
                sessionFound.isLocked = !sessionFound.isLocked;
                return sessionFound.isLocked;
            }
            return false;
        }

        public static bool IsSessionLocked(String session)
        {
            Session sessionFound = sessions.FirstOrDefault(x => x.sessionId == session);
            if (sessionFound != null)
            {
                return sessionFound.isLocked;
            }
            return false;
        }

        public static void RemoveSession(String session)
        {
            sessions.Remove(sessions.FirstOrDefault(x => x.sessionId == session));
        }
    }
}
