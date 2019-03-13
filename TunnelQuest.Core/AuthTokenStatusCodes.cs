using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelQuest.Core
{
    public static class AuthTokenStatusCodes
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Declined = "Declined";

        public static readonly IEnumerable<string> All = new string[] { Pending, Approved, Declined };
    }
}
