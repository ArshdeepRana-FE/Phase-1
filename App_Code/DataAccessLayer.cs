using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class DataAccessLayer
{
    private string connectionString;
    public DataAccessLayer()
    {
        connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
    public DataTable ExecuteQuery(string query)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
    public int ExecuteNonQuery(string query)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}