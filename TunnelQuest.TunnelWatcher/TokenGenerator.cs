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

        public static string GetAuthToken()
        {
            string tokenValue = "";
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (tokenValue == "")
                {
                    //Get only the first CPU's ID
                    tokenValue = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(tokenValue))
                throw new Exception("Could not find computer's processor ID");

            // STUB TODO - do some sort of one-way hash so that we're not sending the server
            // our actual processor ID, but instead some random string seeded by the processor ID.

            return tokenValue;
        }
    }
}
