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
#if DEBUG
            Connection.ExecuteNonQuery("INSERT INTO CardsAmount (Amount) VALUES (1221)");
#endif
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

            Connection.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS [GameCards] (
                [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                [FileId] NVARCHAR(1024) NOT NULL,
                [UniqueFileId] NVARCHAR(1024) NOT NULL);");

            Connection.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS [CardsAmount] (
                [Amount] INTEGER NOT NULL PRIMARY KEY);");
        }
    }
}
