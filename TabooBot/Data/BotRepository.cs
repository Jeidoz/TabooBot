using System.Data.SQLite;
using System.IO;
using TabooBot.Extensions;

namespace TabooBot.Data
{
    public sealed class BotRepository
    {
        private const string DatabaseFileName = "BotDb.db3";
        private readonly string ConnectionString = Path.Combine(Directory.GetCurrentDirectory(), DatabaseFileName);

        public readonly SQLiteConnection Connection;

        public BotRepository()
        {
#if DEBUG
            if(File.Exists(ConnectionString))
            {
                File.Delete(ConnectionString);
            }
#endif
            CreateDatabaseIfNotExists();
            Connection = new SQLiteConnection($"Data Source={ConnectionString};Version=3;");
            Connection.Open();
            CreateAndSeedTablesIfNotExist();
        }
        private void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(ConnectionString))
            {
                SQLiteConnection.CreateFile(ConnectionString);
            }
        }
        private void CreateAndSeedTablesIfNotExist()
        {
            Connection.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS [ChatIdUsernamePairs] (
                [ChatId] INTEGER NOT NULL PRIMARY KEY,
                [UserId] INTEGER NOT NULL,
                [Username] NVARCHAR(128) NOT NULL);");
        }
    }
}
