using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy;

namespace Perro
{
    internal static class ServiceManager
    {
        public const string BUDDY_APP_NAME = "perro";
        public const string BUDDY_APP_KEY = "7260CDD0-C477-4936-B5E3-0607EF41C4A8";

        private static BuddyClient m_client;
        public static BuddyClient Client
        {
            get
            {
                if (m_client == null)
                    m_client = new BuddyClient(BUDDY_APP_NAME, BUDDY_APP_KEY);
                return m_client;
            }
        }

        //Buddy Authenticated user
        public static AuthenticatedUser User;

    }
}
