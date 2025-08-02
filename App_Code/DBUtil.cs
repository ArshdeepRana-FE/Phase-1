using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public static class DBUtil
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

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
