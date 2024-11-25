using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospodaUBobra
{
    public static class UserSession
    {
        public static int UserID { get; set; } = 0;
        public static string Role { get; set; } = "Anonymous";
        

        public static void ClearSession()
        {
            UserID = 0;
            Role = "Anonymous";
        }
    }
}
