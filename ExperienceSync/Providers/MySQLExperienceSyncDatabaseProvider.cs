using Dapper;
using ExperienceSync.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceSync.Providers
{
    public class MySQLExperienceSyncDatabaseProvider
    {
        private ExperienceSync pluginInstance = ExperienceSync.Instance;
        private MySqlConnection connection => new MySqlConnection(connectionString);

        public MySQLExperienceSyncDatabaseProvider()
        {
            Reload();
        }

        private string connectionString => string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};",
            pluginInstance.Configuration.Instance.DatabaseAddress,
            pluginInstance.Configuration.Instance.DatabasePort,
            pluginInstance.Configuration.Instance.DatabaseName,
            pluginInstance.Configuration.Instance.DatabaseUsername,
            pluginInstance.Configuration.Instance.DatabasePassword);

        private string Query(string sql) => sql.Replace("ExperienceSync", pluginInstance.Configuration.Instance.DatabaseTableName);

        public Account GetAccount(string playerId)
        {
            const string sql = "SELECT * FROM ExperienceSync WHERE playerId = @playerId;";

            return connection.QueryFirstOrDefault<Account>(Query(sql), new
            {
                playerId
            });
        }

        public void CreateAccount(Account account)
        {
            const string sql = "INSER INTO ExperienceSync (playerId, experience) VALUES (@playerId, @experience);";

            connection.Execute(Query(sql), account);
        }

        public void SetExperience(string playerId, uint experience)
        {
            const string sql = "UPDATE ExperienceSync SET experience = @experience WHERE playerId = @playerId;";

            connection.Execute(Query(sql), new
            {
                experience,
                playerId
            });
        }

        private void Reload()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS ExperienceSync (playerId VARCHAR(32) NOT NULL, experience int(32) NOT NULL);";

            connection.Execute(Query(sql));
        }
    }
}
