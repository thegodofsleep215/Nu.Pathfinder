using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace pfsim
{
    internal static class SettingsManager
    {
        public static bool Verbose { get; set; }

        public static void SetSettings(string[] settings)
        {
            foreach (string key in ConfigurationManager.AppSettings.Keys)
            {
                if (!SetKey(key, ConfigurationManager.AppSettings[key]))
                    throw new ArgumentException(key);
            }

            // Override default settings
            CommandLineArgReader reader = new CommandLineArgReader(settings);

            foreach (string key in reader.ParsedArguments.Keys)
            {
                if (!SetKey(key, reader[key]))
                    throw new ArgumentException(key);
            }
        }

        public static int PollingPeriod { get; set; }

        internal static bool SetKey(string key, string value)
        {
            bool retval = true;

            switch (key)
            {
                case "Verbose":
                    bool temp_bool;
                    if (bool.TryParse(value, out temp_bool))
                        Verbose = temp_bool;
                    else
                    {
                        //FileLogger.Instance.WriteMessage("Invalid Arguments: Verbose is not a true/false value.  Using default.");
                        Verbose = true; // 2 minutes
                    }
                    break;
                default:
                    retval = false;
                    break;
            }

            return retval;
        }
    }
}
