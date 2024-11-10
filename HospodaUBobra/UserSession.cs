using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospodaUBobra
{
    public static class UserSession
    {
        public static string Username { get; set; } = "Anonymous";
        public static string Role { get; set; } = "Anonymous";
        public static Dictionary<string, Image> ProfilePicturesCache { get; private set; } = new Dictionary<string, Image>();

        public static void CacheProfilePicture(string username, Image profilePicture)
        {
            if (!ProfilePicturesCache.ContainsKey(username))
            {
                ProfilePicturesCache[username] = profilePicture;
            }
        }

        public static Image GetCachedProfilePicture(string username)
        {
            ProfilePicturesCache.TryGetValue(username, out Image profilePicture);
            return profilePicture;
        }

        public static void ClearSession()
        {
            Username = "Anonymous";
            Role = "Anonymous";
        }
    }
}
