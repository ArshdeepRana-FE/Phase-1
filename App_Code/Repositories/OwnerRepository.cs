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
    public List<Restaurant> GetRestaurantsByOwnerId(int ownerId)
    {
        List<Restaurant> restaurants = new List<Restaurant>();

        using (SqlConnection conn = new SqlConnection(DBUtil.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(OwnerQueries.GetRestaurantByOwnerId, conn))
        {
            cmd.Parameters.AddWithValue("@OwnerId", ownerId);
            conn.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    restaurants.Add(
                       new Restaurant
                       {
                           Id = Convert.ToInt32(reader["id"]),
                           Name = reader["name"].ToString()
                       }
                    );
                }
            }
        }

        return restaurants;
    }
}
