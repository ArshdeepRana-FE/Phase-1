using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class OwnerRepository
{
    /// <summary>
    /// Retrieves all restaurant IDs owned by the specified owner.
    /// </summary>
    /// <param name="ownerId">The user ID of the restaurant owner.</param>
    /// <summary>
    /// Retrieves the restaurants associated with an owner.
    /// </summary>
    /// <param name="ownerId">The identifier of the owner.</param>
    /// <returns>The restaurants associated with the specified owner.</returns>
    public List<Restaurant> GetRestaurantsByOwnerId(int ownerId)
    {
        List<Restaurant> restaurants = new List<Restaurant>();

        SqlParameter[] parameters = new SqlParameter[]
        {
        new SqlParameter("@OwnerId", ownerId)
        };

        using (SqlDataReader reader = DBUtil.ExecuteReader(OwnerQueries.GetRestaurantByOwnerId, parameters))
        {
            while (reader.Read())
            {
                restaurants.Add(new Restaurant
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString()
                });
            }
        }

        return restaurants;
    }

}
