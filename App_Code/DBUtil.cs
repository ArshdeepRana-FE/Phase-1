using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Utility class for executing common database operations.
/// </summary>
public static class DBUtil
{
    /// <summary>
    /// Connection string for the RestaurantDB database, read from the configuration file.
    /// </summary>
    public static string ConnectionString =>
       ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

    /// <summary>
    /// Executes a SQL query and returns a single value (the first column of the first row in the result set).
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">An array of SQL parameters to include in the query.</param>
    /// <returns>
    /// The first column of the first row in the result set, or null if the result set is empty.
    /// </returns>
    public static object ExecuteScalar(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }

    /// <summary>
    ///    Executes a SQL query that does not return any data (e.g., INSERT, UPDATE, DELETE).
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    public static void ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///    Executes a SQL query and returns a SqlDataReader for reading the result set.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddRange(parameters);
        conn.Open();

        return cmd.ExecuteReader(CommandBehavior.CloseConnection); // Ensures conn is closed with reader
    }

}
