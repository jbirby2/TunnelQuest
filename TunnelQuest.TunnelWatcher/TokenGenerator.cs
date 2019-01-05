using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TunnelQuest.TunnelWatcher
{
    internal static class TokenGenerator
    {
        private static string cachedToken = null;

        public static string GetAuthToken()
        {
            if (cachedToken != null)
                return cachedToken;

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cachedToken == null)
                {
                    //Get only the first CPU's ID
                    cachedToken = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(cachedToken))
                throw new Exception("Could not find computer's processor ID");

            // STUB TODO - do some sort of one-way hash so that we're not sending the server
            // our actual processor ID, but instead some random string seeded by the processor ID.

            return cachedToken;
        }
    }
}
