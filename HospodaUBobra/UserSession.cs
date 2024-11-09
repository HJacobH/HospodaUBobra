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

        public static void ClearSession()
        {
            Username = "Anonymous";
            Role = "Anonymous";
        }
    }
}
