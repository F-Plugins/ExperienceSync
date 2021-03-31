using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceSync.Providers;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Core.Logging;

namespace ExperienceSync
{
    public class ExperienceSync : RocketPlugin<Configuration>
    {
        public static ExperienceSync Instance { get; private set; }
        public MySQLExperienceSyncDatabaseProvider Database { get; private set; }
        protected override void Load()
        {
            Instance = this;
            Database = new MySQLExperienceSyncDatabaseProvider();
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerUpdateExperience += UnturnedPlayerEvents_OnPlayerUpdateExperience;
            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded");
            Logger.Log($"Get more plugins at unturnedstore.com", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerUpdateExperience -= UnturnedPlayerEvents_OnPlayerUpdateExperience;
            Logger.Log($"{Name} {Assembly.GetName().Version} has been unloaded");
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateExperience(Rocket.Unturned.Player.UnturnedPlayer player, uint experience)
        {
            Database.SetExperience(player.Id, experience);
        }

        private void Events_OnPlayerConnected(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            var account = Database.GetAccount(player.Id);
            if (account == null)
            {
                Database.CreateAccount(new Models.Account 
                {
                    playerId = player.Id,
                    playerExperience = player.Experience
                });
            }
            else
            {
                player.Experience = account.playerExperience;
            }
        }
    }
}
