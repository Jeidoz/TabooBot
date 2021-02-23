using Dapper;
using System.Data.SQLite;
using System.Linq;
using TabooBot.Data.Models;

namespace TabooBot.Extensions
{
    public static class DatabaseModelExtensions
    {
        public static void SaveAsNewPair(this ChatIdToUsernamePair model, SQLiteConnection connection)
        {
            connection.ExecuteNonQuery(@"
                INSERT INTO ChatIdUsernamePairs (ChatId, UserId, Username)
                VALUES (@ChatId, @UserId, @Username)",
                model);
        }

        public static bool ExistsInDatabaseByUserId(this ChatIdToUsernamePair pair, SQLiteConnection connection)
        {
            string sql = @$"
                SELECT COUNT(1) AS 'Count' 
                FROM ChatIdUsernamePairs 
                WHERE UserId = {pair.UserId}";
            var rows = connection.Query(sql);
            return (int)(rows.First().Count) > 0;
        }
        public static bool ExistsInDatabaseByUsername(this ChatIdToUsernamePair pair, SQLiteConnection connection)
        {
            string sql = @$"
                SELECT COUNT(1) AS 'Count' 
                FROM ChatIdUsernamePairs 
                WHERE Username = {pair.Username}";
            var rows = connection.Query(sql);
            return (int)(rows.First().Count) > 0;
        }
        public static bool ExistsInDatabaseByChatId(this ChatIdToUsernamePair pair, SQLiteConnection connection)
        {
            string sql = @$"
                SELECT COUNT(1) AS 'Count' 
                FROM ChatIdUsernamePairs 
                WHERE ChatId = {pair.ChatId}";
            var rows = connection.Query(sql);
            return (int)(rows.First().Count) > 0;
        }

        public static void SaveNewGameCard(this GameCard card, SQLiteConnection connection)
        {
            connection.ExecuteNonQuery(@"
                INSERT INTO GameCards (FileId, UniqueFileId)
                VALUES (@FileId, @UniqueFileId)",
                card);
        }
        public static GameCard GetGameCardById(this SQLiteConnection connection, int id)
        {
            var sql = @$"
                SELECT *
                FROM GameCards
                WHERE Id = {id}";
            return connection.Query<GameCard>(sql)
                .FirstOrDefault();
        }
        public static GameCard GetGameCardByFileId(this SQLiteConnection connection, int fileId)
        {
            var sql = @$"
                SELECT *
                FROM GameCards
                WHERE UniqueFileId = {fileId}";
            return connection.Query<GameCard>(sql)
                .FirstOrDefault();
        }
        public static bool ExistsInDatabaseByFileId(this GameCard card, SQLiteConnection connection)
        {
            var sql = @$"
                SELECT COUNT(1) AS 'Count'
                FROM GameCards
                WHERE UniqueFileId = '{card.UniqueFileId}'";
            var rows = connection.Query(sql);
            return (int)(rows.First().Count) > 0;
        }
    }
}