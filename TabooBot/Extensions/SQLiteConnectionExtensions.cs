using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace TabooBot.Extensions
{
    public static class SQLiteConnectionExtensions
    {

        public static void ExecuteNonQuery(this SQLiteConnection connection, string commandText)
        {
            // Ensure we have a connection
            if (connection is null)
            {
                throw new NullReferenceException("Please provide a connection");
            }

            // Ensure that the connection state is Open
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // Use Dapper to execute the given query
            connection.Execute(commandText);
        }

        // Insert/Update queries
        public static void ExecuteNonQuery(this SQLiteConnection connection,
            string commandText, object param = null)
        {
            // Ensure we have a connection
            if (connection is null)
            {
                throw new NullReferenceException("Please provide a connection");
            }

            // Ensure that the connection state is Open
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // Use Dapper to execute the given query
            connection.Execute(commandText, param);
        }
    }
}
