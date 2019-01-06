using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Data
{
    public static class AuthTokenStatusCodes
    {
        public static readonly string Pending = "Pending";
        public static readonly string Approved = "Approved";
        public static readonly string Declined = "Declined";

        public static readonly IEnumerable<string> All = new string[] { Pending, Approved, Declined };
    }
}
