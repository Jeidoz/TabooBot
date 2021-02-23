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
        public static int GetCardsAmount(this SQLiteConnection connection)
        {
            var rows = connection.Query("SELECT * FROM CardsAmount");
            return (int)rows.First().Amount;
        }
        public static void UpdateCardsAmount(this SQLiteConnection connection, int amount)
        {
            connection.ExecuteNonQuery("UPDATE CardsAmount SET Amount = @Amount", new { Amount = amount });
        }
    }
}