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
    private static string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

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
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
