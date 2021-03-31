using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceSync
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string DatabaseAddress { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseTableName { get; set; }
        public int DatabasePort { get; set; }

        public void LoadDefaults()
        {
            DatabaseAddress = "localhost";
            DatabaseUsername = "unturned";
            DatabasePassword = "password";
            DatabaseName = "unturned";
            DatabaseTableName = "ExperienceSync";
            DatabasePort = 3306;
        }
    }
}
