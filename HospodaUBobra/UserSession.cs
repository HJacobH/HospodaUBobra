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
        public static string TableName { get; set; }

        public static string EmulatedRole { get; set; }

        public static string CurrentRole
        {
            get
            {
                // Use emulated role if set, otherwise fallback to actual role
                return EmulatedRole ?? Role;
            }
            set
            {

            }
        }

        public static void ClearSession()
        {
            UserID = 0;
            Role = "Anonymous";
            CurrentRole = Role;
            EmulatedRole = null;
        }
    }
}
