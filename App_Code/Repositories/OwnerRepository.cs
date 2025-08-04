using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class OwnerRepository
{
    /// <summary>
    /// Retrieves all restaurant IDs owned by the specified owner.
    /// </summary>
    /// <param name="ownerId">The user ID of the restaurant owner.</param>
    /// <returns>A list of OwnerRestaurant instances.</returns>
    public List<int> GetRestaurantsByOwnerId(int ownerId)
    {
        var restaurants = new List<int>();

        string query = @"
            SELECT id 
            FROM owner_restaurant 
            WHERE owner_id = @OwnerId";

        using (SqlConnection conn = new SqlConnection(DBUtil.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@OwnerId", ownerId);
            conn.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    restaurants.Add(
                        Convert.ToInt32(reader["id"])
                    );
                }
            }
        }

        return restaurants;
    }
}
